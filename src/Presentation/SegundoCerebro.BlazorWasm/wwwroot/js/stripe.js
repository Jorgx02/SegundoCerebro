// Este archivo actúa como un puente entre Blazor (C#) y la librería Stripe.js.

let stripe;
let cardElement;

// Inicializa Stripe.js con la clave publicable y crea el elemento de la tarjeta.
export function initStripe(publishableKey) {
    stripe = Stripe(publishableKey);
    const elements = stripe.elements();

    const style = {
        base: {
            color: '#32325d',
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: 'antialiased',
            fontSize: '16px',
            '::placeholder': {
                color: '#aab7c4'
            }
        },
        invalid: {
            color: '#fa755a',
            iconColor: '#fa755a'
        }
    };

    cardElement = elements.create('card', { style: style });
}

// Monta el elemento de la tarjeta en un div específico del DOM.
export function mountCardElement(elementId) {
    if (cardElement) {
        cardElement.mount(`#${elementId}`);
    }
}

// Desmonta el elemento de la tarjeta para limpiar la vista.
export function unmountCardElement() {
    if (cardElement) {
        cardElement.unmount();
    }
}

// Crea un PaymentMethod de Stripe y devuelve el resultado (ID o error) a Blazor.
export async function createPaymentMethod(dotNetObjectReference, cardholderName) {
    if (!stripe || !cardElement) return;

    const { paymentMethod, error } = await stripe.createPaymentMethod({ type: 'card', card: cardElement, billing_details: { name: cardholderName } });

    if (error) {
        await dotNetObjectReference.invokeMethodAsync('OnStripeError', error.message);
    } else {
        await dotNetObjectReference.invokeMethodAsync('OnStripeSuccess', paymentMethod.id);
    }
}