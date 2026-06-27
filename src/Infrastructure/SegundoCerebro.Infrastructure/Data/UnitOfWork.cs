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

    // Private fields for lazy initialization of repositories
    private IAccountRepository? _accountRepository;
    private ITransactionRepository? _transactionRepository;
    private ICategoryRepository? _categoryRepository;
    private IBudgetRepository? _budgetRepository;
    private IProjectRepository? _projectRepository;
    private ITodoItemRepository? _todoItemRepository;
    private ICardRepository? _cardRepository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="UnitOfWork"/>.
    /// </summary>
    /// <param name="context">El contexto de la base de datos.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public IAccountRepository Accounts => _accountRepository ??= new AccountRepository(_context);
    /// <inheritdoc />
    public ITransactionRepository Transactions => _transactionRepository ??= new TransactionRepository(_context);
    /// <inheritdoc />
    public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
    /// <inheritdoc />
    public IBudgetRepository Budgets => _budgetRepository ??= new BudgetRepository(_context);
    /// <inheritdoc />
    public IProjectRepository Projects => _projectRepository ??= new ProjectRepository(_context);
    /// <inheritdoc />
    public ITodoItemRepository TodoItems => _todoItemRepository ??= new TodoItemRepository(_context);
    /// <inheritdoc />
    public ICardRepository Cards => _cardRepository ??= new CardRepository(_context);

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