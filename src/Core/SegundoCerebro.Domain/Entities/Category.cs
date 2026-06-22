using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una categoría para clasificar transacciones (ingresos o gastos).
/// Soporta una estructura jerárquica a través de la auto-referencia `ParentCategory`.
/// </summary>
public class Category
{
    /// <summary>Identificador único de la categoría.</summary>
    public Guid Id { get; set; }

    /// <summary>Nombre de la categoría (ej. "Comida", "Transporte").</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Descripción opcional para aclarar el propósito de la categoría.</summary>
    public string? Description { get; set; }

    /// <summary>Color en formato hexadecimal (ej. "#RRGGBB") para la representación visual en la UI.</summary>
    public string Color { get; set; } = "#3B82F6"; // Default blue color

    /// <summary>Nombre o clase de un icono (ej. de FontAwesome) para la representación visual.</summary>
    public string Icon { get; set; } = "fas fa-folder"; // Font Awesome icon

    /// <summary>Define si la categoría es para Ingresos (`Income`) o Gastos (`Expense`).</summary>
    public CategoryType Type { get; set; }

    /// <summary>Indica si la categoría está disponible para ser usada (Soft Delete).</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Fecha de creación del registro.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Fecha de la última actualización del registro.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Clave foránea opcional a la categoría padre, para crear jerarquías.</summary>
    public Guid? ParentCategoryId { get; set; }

    // Propiedades de Navegación (EF Core)
    /// <summary>Propiedad de navegación a la categoría padre.</summary>
    public Category? ParentCategory { get; set; }

    /// <summary>Colección de subcategorías que dependen de esta categoría.</summary>
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    /// <summary>Colección de transacciones clasificadas bajo esta categoría.</summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>Colección de presupuestos definidos para esta categoría.</summary>
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}