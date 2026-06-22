using Microsoft.EntityFrameworkCore;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

/// <summary>
/// Implementación del repositorio para la entidad Presupuesto (`Budget`).
/// </summary>
public class BudgetRepository : Repository<Budget>, IBudgetRepository
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="BudgetRepository"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public BudgetRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtiene todos los presupuestos que están activos y cuya fecha de fin no ha pasado.
    /// </summary>
    public async Task<IEnumerable<Budget>> GetActiveBudgetsAsync()
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive && b.EndDate >= DateTime.Now)
            .OrderBy(b => b.Category.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene todos los presupuestos activos donde el gasto (`Spent`) ha superado el monto (`Amount`).
    /// </summary>
    public async Task<IEnumerable<Budget>> GetOverBudgetsAsync()
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.IsActive && b.Spent > b.Amount)
            .OrderByDescending(b => b.Spent - b.Amount)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene los presupuestos cuyo período se encuentra dentro de un rango de fechas específico.
    /// </summary>
    public async Task<IEnumerable<Budget>> GetByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.StartDate >= startDate && b.EndDate <= endDate)
            .OrderBy(b => b.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un presupuesto por su ID, incluyendo sus entidades relacionadas (Categoría y Cuenta).
    /// </summary>
    public async Task<Budget?> GetBudgetWithDetailsAsync(Guid id)
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    /// <summary>
    /// Busca un presupuesto activo para una categoría específica en una fecha determinada.
    /// </summary>
    /// <param name="categoryId">El ID de la categoría.</param>
    /// <param name="date">La fecha que debe estar dentro del período del presupuesto.</param>
    public async Task<Budget?> GetBudgetByCategoryAndPeriodAsync(Guid categoryId, DateTime date)
    {
        return await _context.Budgets
            .Include(b => b.Category)
            .Include(b => b.Account)
            .Where(b => b.CategoryId == categoryId
                     && b.IsActive
                     && b.StartDate <= date
                     && b.EndDate >= date)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Actualiza el campo `Spent` de un presupuesto, sumando o restando un monto.
    /// </summary>
    /// <param name="budgetId">El ID del presupuesto a actualizar.</param>
    /// <param name="amount">El monto a añadir (puede ser negativo para revertir un gasto).</param>
    public async Task UpdateBudgetSpentAsync(Guid budgetId, decimal amount)
    {
        var budget = await _context.Budgets.FindAsync(budgetId);
        if (budget != null)
        {
            budget.Spent += amount;
            budget.UpdatedAt = DateTime.UtcNow;
            _context.Budgets.Update(budget);
        }
        await Task.CompletedTask;
    }
}