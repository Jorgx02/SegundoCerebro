using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Card.
/// </summary>
public class CardRepository : Repository<Card>, ICardRepository
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="CardRepository"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public CardRepository(ApplicationDbContext context) : base(context)
    {
        _dbContext = context;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Card>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbContext.Cards
            .Where(c => c.AccountId == accountId)
            .ToListAsync();
    }
}