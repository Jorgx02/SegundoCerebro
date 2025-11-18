using Microsoft.EntityFrameworkCore.Storage;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Repositories;

namespace SegundoCerebro.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Accounts = new AccountRepository(_context);
        Transactions = new TransactionRepository(_context);
        Categories = new CategoryRepository(_context);
        Budgets = new BudgetRepository(_context);
    }

    public IAccountRepository Accounts { get; }
    public ITransactionRepository Transactions { get; }
    public ICategoryRepository Categories { get; }
    public IBudgetRepository Budgets { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}