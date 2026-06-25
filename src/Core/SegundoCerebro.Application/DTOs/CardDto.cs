namespace SegundoCerebro.Application.DTOs;

/// <summary>
/// Data Transfer Object para la entidad Card.
/// Contiene la información pública de una tarjeta que se puede enviar a los clientes de la API.
/// </summary>
public class CardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Last4Digits { get; set; } = string.Empty;
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public Guid AccountId { get; set; }
}