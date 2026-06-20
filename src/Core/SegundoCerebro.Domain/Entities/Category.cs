using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una categoría de gasto o ingreso, que puede ser jerárquica (subcategorías) y se utiliza para clasificar transacciones y presupuestos.
/// </summary>
public class Category
{
    /// <summary>Obtiene o establece el identificador único de la categoría.</summary>
    public Guid Id { get; set; }

    /// <summary>Obtiene o establece el nombre de la categoría.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Obtiene o establece la descripción de la categoría.</summary>
    public string? Description { get; set; }

    /// <summary>Obtiene o establece el color de la categoría.</summary>
    public string Color { get; set; } = "#3B82F6"; // Default blue color

    /// <summary>Obtiene o establece el icono de la categoría.</summary>
    public string Icon { get; set; } = "fas fa-folder"; // Font Awesome icon

    /// <summary>Obtiene o establece el tipo de la categoría.</summary>
    public CategoryType Type { get; set; }

    /// <summary>Obtiene o establece si la categoría está activa o no.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Obtiene o establece la fecha de creación de la categoría.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Obtiene o establece la fecha de actualización de la categoría.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Obtiene o establece el identificador de la categoría principal.</summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>Obtiene o establece la categoría principal.</summary>
    public Category? ParentCategory { get; set; }

    /// <summary>Obtiene o establece las subcategorías.</summary>
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    /// <summary>Obtiene o establece las transacciones asociadas a la categoría.</summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>Obtiene o establece los presupuestos asociados a la categoría.</summary>
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}