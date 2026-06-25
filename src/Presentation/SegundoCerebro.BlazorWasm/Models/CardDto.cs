namespace SegundoCerebro.BlazorWasm.Models;

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