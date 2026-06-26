namespace SegundoCerebro.BlazorWasm.Models.Commands;

/// <summary>
/// Comando del frontend para crear una tarjeta.
/// </summary>
public class CreateCardCommand
{
    public Guid AccountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string StripePaymentMethodId { get; set; } = string.Empty;
    // Propiedad solo para el formulario del frontend, no se envía directamente al backend en este campo.
    public string CardholderName { get; set; } = string.Empty;
}