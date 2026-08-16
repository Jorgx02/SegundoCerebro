using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el repositorio de Entradas de Bienestar (WellnessEntry).
/// </summary>
public interface IWellnessEntryRepository : IRepository<WellnessEntry>
{
}