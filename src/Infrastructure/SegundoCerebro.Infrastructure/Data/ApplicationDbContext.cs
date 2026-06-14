using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Application.Interfaces;

namespace SegundoCerebro.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Budget> Budgets => Set<Budget>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Account configuration
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
            entity.Property(a => a.UserId).IsRequired().HasMaxLength(450); // Mismo tamaño que IdentityUser.Id
            entity.Property(a => a.Currency).IsRequired().HasMaxLength(3);
            entity.Property(a => a.Balance).HasPrecision(18, 2);

            // Filtro Global: Solo devuelve cuentas de este usuario
            entity.HasQueryFilter(a => _currentUserService.UserId == null || a.UserId == _currentUserService.UserId);
        });

        // Transaction configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Amount).HasPrecision(18, 2);
            entity.Property(t => t.UserId).IsRequired().HasMaxLength(450);

            entity.HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Filtro Global: Solo devuelve transacciones de este usuario
            entity.HasQueryFilter(t => _currentUserService.UserId == null || t.UserId == _currentUserService.UserId);
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Color).HasMaxLength(7);
            entity.Property(c => c.Icon).HasMaxLength(50);

            entity.HasOne(c => c.ParentCategory)
                .WithMany()
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Budget configuration
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired().HasMaxLength(100);
            entity.Property(b => b.Amount).HasPrecision(18, 2);
            entity.Property(b => b.Spent).HasPrecision(18, 2);
            entity.Property(b => b.UserId).IsRequired().HasMaxLength(450);

            entity.HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Account)
                .WithMany(a => a.Budgets)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            // Filtro Global: Solo devuelve presupuestos de este usuario
            entity.HasQueryFilter(b => _currentUserService.UserId == null || b.UserId == _currentUserService.UserId);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        // Si hay un usuario logueado, le asignamos la propiedad a las entidades nuevas automáticamente
        if (!string.IsNullOrEmpty(userId))
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is Account account && string.IsNullOrEmpty(account.UserId))
                        account.UserId = userId;
                    else if (entry.Entity is Transaction transaction && string.IsNullOrEmpty(transaction.UserId))
                        transaction.UserId = userId;
                    else if (entry.Entity is Budget budget && string.IsNullOrEmpty(budget.UserId))
                        budget.UserId = userId;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}