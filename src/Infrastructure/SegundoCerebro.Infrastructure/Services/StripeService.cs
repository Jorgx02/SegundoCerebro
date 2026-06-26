using Microsoft.Extensions.Options;
using SegundoCerebro.Application.Interfaces;
using SegundoCerebro.Infrastructure.Settings;
using Stripe;

namespace SegundoCerebro.Infrastructure.Services;

/// <summary>
/// Implementación del servicio para interactuar con la API de Stripe.
/// </summary>
public class StripeService : IStripeService
{
    public StripeService(IOptions<StripeSettings> stripeSettings)
    {
        // Configura la clave de API global para Stripe en el momento de la inyección.
        StripeConfiguration.ApiKey = stripeSettings.Value.SecretKey;
    }

    public async Task<StripeCardDetails> GetCardDetailsFromPaymentMethodAsync(string paymentMethodId)
    {
        var service = new PaymentMethodService();
        var paymentMethod = await service.GetAsync(paymentMethodId);

        if (paymentMethod?.Card == null)
        {
            throw new InvalidOperationException("The provided Stripe Payment Method is not a valid card.");
        }

        return new StripeCardDetails
        {
            Brand = paymentMethod.Card.Brand,
            Last4Digits = paymentMethod.Card.Last4,
            ExpirationMonth = (int)paymentMethod.Card.ExpMonth,
            ExpirationYear = (int)paymentMethod.Card.ExpYear
        };
    }
}