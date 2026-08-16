using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad WellnessEntry.
/// </summary>
public class WellnessEntryRepository : Repository<WellnessEntry>, IWellnessEntryRepository
{
    public WellnessEntryRepository(ApplicationDbContext context) : base(context)
    {
    }
}