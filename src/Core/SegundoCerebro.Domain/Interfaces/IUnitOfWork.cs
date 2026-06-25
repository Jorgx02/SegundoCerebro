using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Domain.Interfaces;

/// <summary>
/// Define el contrato para el patrón Unit of Work.
/// Centraliza el acceso a todos los repositorios y gestiona las transacciones de la base de datos
/// para garantizar la consistencia de los datos en operaciones complejas.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repositorio para las entidades de Cuentas (Account).
    /// </summary>
    IAccountRepository Accounts { get; }
    /// <summary>
    /// Repositorio para las entidades de Transacciones (Transaction).
    /// </summary>
    ITransactionRepository Transactions { get; }
    /// <summary>
    /// Repositorio para las entidades de Categorías (Category).
    /// </summary>
    ICategoryRepository Categories { get; }
    /// <summary>
    /// Repositorio para las entidades de Presupuestos (Budget).
    /// </summary>
    IBudgetRepository Budgets { get; }
    /// <summary>
    /// Repositorio para las entidades de Proyectos (Project).
    /// </summary>
    IProjectRepository Projects { get; }
    /// <summary>
    /// Repositorio para las entidades de Tareas (TodoItem).
    /// </summary>
    ITodoItemRepository TodoItems { get; }

    /// <summary>
    /// Repositorio para las entidades de Tarjetas (Card).
    /// </summary>
    ICardRepository Cards { get; }

    /// <summary>
    /// Guarda todos los cambios realizados en el contexto de la base de datos.
    /// </summary>
    /// <returns>El número de entidades afectadas.</returns>
    Task<int> SaveChangesAsync();
    /// <summary>Inicia una nueva transacción de base de datos.</summary>
    Task BeginTransactionAsync();
    /// <summary>Confirma todos los cambios realizados durante la transacción actual.</summary>
    Task CommitTransactionAsync();
    /// <summary>Revierte todos los cambios realizados durante la transacción actual.</summary>
    Task RollbackTransactionAsync();
}