using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Application.Interfaces;

namespace SegundoCerebro.Infrastructure.Data;

/// <summary>
/// Contexto de la base de datos principal de la aplicación.
/// Hereda de IdentityDbContext para integrar la gestión de usuarios de ASP.NET Core Identity.
/// Es el punto de entrada para todas las operaciones de base de datos con Entity Framework Core.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="ApplicationDbContext"/>.
    /// </summary>
    /// <param name="options">Las opciones para configurar el contexto.</param>
    /// <param name="currentUserService">Servicio para obtener el ID del usuario actualmente autenticado,
    /// crucial para implementar la seguridad de datos (Multi-tenancy).</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    // DbSets para cada entidad del dominio.
    // Representan las tablas en la base de datos.
    /// <summary>Conjunto de entidades para las Cuentas (`Account`).</summary>
    public DbSet<Account> Accounts => Set<Account>();

    /// <summary>Conjunto de entidades para las Transacciones (`Transaction`).</summary>
    public DbSet<Transaction> Transactions => Set<Transaction>();

    /// <summary>Conjunto de entidades para las Categorías (`Category`).</summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>Conjunto de entidades para los Presupuestos (`Budget`).</summary>
    public DbSet<Budget> Budgets => Set<Budget>();

    /// <summary>Conjunto de entidades para los Proyectos (`Project`).</summary>
    public DbSet<Project> Projects => Set<Project>();

    /// <summary>Conjunto de entidades para las Tareas (`TodoItem`).</summary>
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    /// <summary>Conjunto de entidades para las Tarjetas (`Card`).</summary>
    public DbSet<Card> Cards => Set<Card>();

    /// <summary>
    /// Configura el modelo de datos, las relaciones, las restricciones y los filtros globales
    /// antes de que sea bloqueado y utilizado para inicializar el contexto.
    /// </summary>
    /// <param name="modelBuilder">El constructor que se utiliza para construir el modelo para este contexto.</param>
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

            // Filtro de Consulta Global (Global Query Filter) para Multi-tenancy:
            // Asegura que cualquier consulta a la tabla 'Accounts' siempre se filtre
            // por el ID del usuario actual, evitando fugas de datos entre usuarios.
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

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.Property(p => p.UserId).IsRequired().HasMaxLength(450);

            // Filtro Global: Solo devuelve proyectos de este usuario
            entity.HasQueryFilter(p => _currentUserService.UserId == null || p.UserId == _currentUserService.UserId);
        });

        // TodoItem configuration
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
            entity.Property(t => t.Description).HasMaxLength(1000);
            entity.Property(t => t.UserId).IsRequired().HasMaxLength(450);

            entity.HasOne(t => t.Project)
                .WithMany(p => p.TodoItems)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // Si se borra el proyecto, sus tareas también se borran.

            // Filtro Global: Solo devuelve tareas de este usuario
            entity.HasQueryFilter(t => _currentUserService.UserId == null || t.UserId == _currentUserService.UserId);
        });

        // Card configuration
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Brand).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Last4Digits).IsRequired().HasMaxLength(4);
            entity.Property(c => c.StripePaymentMethodId).IsRequired();
            entity.Property(c => c.UserId).IsRequired().HasMaxLength(450);

            // Relationship with Account
            entity.HasOne(c => c.Account)
                .WithMany(a => a.Cards)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade); // Si una cuenta se borra físicamente, sus tarjetas también.

            // Global Query Filter for Multi-tenancy
            entity.HasQueryFilter(c => _currentUserService.UserId == null || c.UserId == _currentUserService.UserId);
        });

    }

    /// <summary>
    /// Sobrescribe el método SaveChangesAsync para interceptar las operaciones de guardado.
    /// Asigna automáticamente el `UserId` del usuario actual a las nuevas entidades
    /// que se están creando, garantizando la propiedad de los datos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El número de entidades escritas en la base de datos.</returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        // Si hay un usuario autenticado, se asigna su ID a las nuevas entidades.
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
                    else if (entry.Entity is Project project && string.IsNullOrEmpty(project.UserId))
                        project.UserId = userId;
                    else if (entry.Entity is TodoItem todoItem && string.IsNullOrEmpty(todoItem.UserId))
                        todoItem.UserId = userId;
                    else if (entry.Entity is Card card && string.IsNullOrEmpty(card.UserId))
                        card.UserId = userId;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}