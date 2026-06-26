namespace SegundoCerebro.BlazorWasm.Models.Commands;

/// <summary>
/// DTO para enviar la solicitud de actualización del nombre de una tarjeta.
/// </summary>
public class UpdateCardCommand
{
    public string Name { get; set; } = string.Empty;
}