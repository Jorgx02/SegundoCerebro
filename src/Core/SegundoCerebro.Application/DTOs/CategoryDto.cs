using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// DTO para representar los datos de una categoría de transacción.
/// </summary>
public class CategoryDto
{
    /// <summary>Identificador único de la categoría.</summary>
    public Guid Id { get; set; }
    /// <summary>Nombre de la categoría.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Color en formato hexadecimal (ej. #RRGGBB) para la UI.</summary>
    public string Color { get; set; } = "#3B82F6";
    /// <summary>Nombre del icono (ej. de FontAwesome o Material Icons) para la UI.</summary>
    public string Icon { get; set; } = "fas fa-folder";
    /// <summary>Tipo de categoría (Ingreso o Gasto).</summary>
    public CategoryType Type { get; set; }
    /// <summary>Nombre legible del tipo de categoría.</summary>
    public string TypeName => Type.ToString();
    /// <summary>Indica si la categoría está activa.</summary>
    public bool IsActive { get; set; }
    /// <summary>Fecha de creación.</summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>Fecha de última actualización.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>ID de la categoría padre, si es una subcategoría.</summary>
    public Guid? ParentCategoryId { get; set; }
    /// <summary>Nombre de la categoría padre.</summary>
    public string? ParentCategoryName { get; set; }

    /// <summary>Colección de subcategorías anidadas.</summary>
    public List<CategoryDto> SubCategories { get; set; } = new();
}

/// <summary>
/// DTO utilizado para crear una nueva categoría.
/// </summary>
public class CreateCategoryDto
{
    /// <summary>Nombre de la nueva categoría.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Color en formato hexadecimal.</summary>
    public string Color { get; set; } = "#3B82F6";
    /// <summary>Nombre del icono.</summary>
    public string Icon { get; set; } = "fas fa-folder";
    /// <summary>Tipo de categoría (Ingreso/Gasto).</summary>
    public CategoryType Type { get; set; }
    /// <summary>ID de la categoría padre opcional.</summary>
    public Guid? ParentCategoryId { get; set; }
}

/// <summary>
/// DTO utilizado para actualizar una categoría existente.
/// </summary>
public class UpdateCategoryDto
{
    /// <summary>Nuevo nombre de la categoría.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Nueva descripción opcional.</summary>
    public string? Description { get; set; }
    /// <summary>Nuevo color en formato hexadecimal.</summary>
    public string Color { get; set; } = "#3B82F6";
    /// <summary>Nuevo nombre del icono.</summary>
    public string Icon { get; set; } = "fas fa-folder";
    /// <summary>Nuevo tipo de categoría.</summary>
    public CategoryType Type { get; set; }
    /// <summary>Nuevo ID de la categoría padre.</summary>
    public Guid? ParentCategoryId { get; set; }
    /// <summary>Nuevo estado de activación.</summary>
    public bool IsActive { get; set; }
}