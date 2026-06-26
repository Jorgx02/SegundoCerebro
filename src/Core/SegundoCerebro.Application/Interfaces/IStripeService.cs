namespace SegundoCerebro.Application.Interfaces;

/// <summary>
/// DTO para transportar los detalles no sensibles de una tarjeta obtenidos de Stripe.
/// </summary>
public class StripeCardDetails
{
    public string Brand { get; set; } = string.Empty;
    public string Last4Digits { get; set; } = string.Empty;
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
}

public interface IStripeService
{
    Task<StripeCardDetails> GetCardDetailsFromPaymentMethodAsync(string paymentMethodId);
}