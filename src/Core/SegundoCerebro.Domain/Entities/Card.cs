namespace SegundoCerebro.Domain.Entities;

/// <summary>
/// Representa una tarjeta de crédito o débito asociada a una cuenta bancaria.
/// No almacena información sensible de la tarjeta, sino metadatos y referencias a la pasarela de pago.
/// </summary>
public class Card
{
    /// <summary>
    /// Identificador único de la tarjeta.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre o alias para la tarjeta (ej. "Visa Trabajo", "Mastercard Personal").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Marca de la tarjeta (ej. "Visa", "Mastercard", "American Express").
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// Últimos 4 dígitos de la tarjeta para identificación visual.
    /// </summary>
    public string Last4Digits { get; set; } = string.Empty;

    /// <summary>
    /// Mes de expiración de la tarjeta (1-12).
    /// </summary>
    public int ExpirationMonth { get; set; }

    /// <summary>
    /// Año de expiración de la tarjeta (ej. 2025).
    /// </summary>
    public int ExpirationYear { get; set; }

    /// <summary>
    /// Identificador del método de pago en la pasarela de Stripe.
    /// Este es el token seguro que se utiliza para realizar cargos.
    /// </summary>
    public string StripePaymentMethodId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador de la cuenta bancaria a la que está vinculada esta tarjeta.
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Propiedad de navegación a la cuenta principal.
    /// </summary>
    public Account Account { get; set; } = null!;

    /// <summary>Identificador del propietario (Aislamiento de datos / Multi-tenant).</summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de la última actualización del registro.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}