using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para un repositorio de la entidad Card.
/// Hereda las operaciones CRUD genéricas de IRepository.
/// </summary>
public interface ICardRepository : IRepository<Card>
{
    // Aquí se pueden añadir en el futuro métodos específicos para las tarjetas,
    // por ejemplo: Task<Card?> GetByStripeIdAsync(string stripeId);

    /// <summary>
    /// Obtiene todas las tarjetas asociadas a un ID de cuenta específico.
    /// </summary>
    /// <param name="accountId">El ID de la cuenta.</param>
    /// <returns>Una colección de tarjetas.</returns>
    Task<IEnumerable<Card>> GetByAccountIdAsync(Guid accountId);
}