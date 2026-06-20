using AutoMapper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Application.Mappings;

/// <summary>
/// Configuración de los mapeos entre entidades del Dominio y DTOs de la Aplicación usando AutoMapper.
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="MappingProfile"/>.
    /// </summary>
    public MappingProfile()
    {
        // Account mappings
        // Mapeo de Entidad a DTO para consultas.
        CreateMap<Account, AccountDto>();
        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateAccountDto, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateAccountDto, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Balance, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        // Transaction mappings
        // Mapeo de Entidad a DTO, incluyendo nombres de entidades relacionadas.
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        // Category mappings
        // Mapeo de Entidad a DTO, incluyendo el nombre de la categoría padre.
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));

        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateCategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateCategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        // Budget mappings
        // Mapeo de Entidad a DTO, incluyendo nombres de entidades relacionadas.
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null));

        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateBudgetDto, Budget>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Spent, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateBudgetDto, Budget>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Spent, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());

        // Project mappings
        // Mapeo simple de Entidad a DTO.
        CreateMap<Project, ProjectDto>();

        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TodoItems, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateProjectDto, Project>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TodoItems, opt => opt.Ignore());

        // TodoItem mappings
        // Mapeo de Entidad a DTO, incluyendo el nombre del proyecto asociado.
        CreateMap<TodoItem, TodoItemDto>()
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : null));

        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateTodoItemDto, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore());

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateTodoItemDto, TodoItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedAt, opt => opt.Ignore()) // Lo manejamos manualmente en el Handler
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Project, opt => opt.Ignore());
    }
}