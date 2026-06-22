using Microsoft.EntityFrameworkCore.Storage;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Repositories;

namespace SegundoCerebro.Infrastructure.Data;

/// <summary>
/// Implementación del patrón Unit of Work.
/// Coordina el trabajo de múltiples repositorios en una única transacción de base de datos.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Accounts = new AccountRepository(_context);
        Transactions = new TransactionRepository(_context);
        Categories = new CategoryRepository(_context);
        Budgets = new BudgetRepository(_context);
        Projects = new ProjectRepository(_context);
        TodoItems = new TodoItemRepository(_context);
    }

    /// <inheritdoc />
    public IAccountRepository Accounts { get; }
    /// <inheritdoc />
    public ITransactionRepository Transactions { get; }
    /// <inheritdoc />
    public ICategoryRepository Categories { get; }
    /// <inheritdoc />
    public IBudgetRepository Budgets { get; }
    /// <inheritdoc />
    public IProjectRepository Projects { get; }
    /// <inheritdoc />
    public ITodoItemRepository TodoItems { get; }

    /// <summary>
    /// Guarda todos los cambios pendientes en el contexto de la base de datos.
    /// </summary>
    /// <returns>El número de entidades afectadas.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Inicia una nueva transacción de base de datos.
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Confirma (commit) la transacción actual, haciendo permanentes todos los cambios.
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Revierte (rollback) la transacción actual, descartando todos los cambios.
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    /// <summary>
    /// Libera los recursos utilizados por el contexto y la transacción.
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}