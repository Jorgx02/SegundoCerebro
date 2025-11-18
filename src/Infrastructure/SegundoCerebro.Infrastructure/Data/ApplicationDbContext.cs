using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Budget> Budgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Account Configuration
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.AccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.Balance).HasPrecision(18, 2);
        });

        // Transaction Configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Reference).HasMaxLength(100);
            
            entity.HasOne(e => e.Account)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Category)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Category Configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Color).HasMaxLength(7);
            entity.Property(e => e.Icon).HasMaxLength(50);
            
            entity.HasOne(e => e.ParentCategory)
                .WithMany(e => e.SubCategories)
                .HasForeignKey(e => e.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Budget Configuration
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Spent).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Category)
                .WithMany(e => e.Budgets)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Account)
                .WithMany(e => e.Budgets)
                .HasForeignKey(e => e.AccountId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}