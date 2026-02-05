using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;
using SegundoCerebro.Infrastructure.Data;

namespace SegundoCerebro.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IGenericRepository<Account>? _accounts;
    private ITransactionRepository? _transactions;
    private IGenericRepository<Category>? _categories;
    private IBudgetRepository? _budgets; // NUEVO

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Account> Accounts => 
        _accounts ??= new GenericRepository<Account>(_context);

    public ITransactionRepository Transactions => 
        _transactions ??= new TransactionRepository(_context);

    public IGenericRepository<Category> Categories => 
        _categories ??= new GenericRepository<Category>(_context);

    public IBudgetRepository Budgets =>  // NUEVO
        _budgets ??= new BudgetRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}