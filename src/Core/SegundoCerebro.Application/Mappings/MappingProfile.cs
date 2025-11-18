using AutoMapper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Account mappings
        CreateMap<Account, AccountDto>();
        CreateMap<CreateAccountDto, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        CreateMap<UpdateAccountDto, Account>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Balance, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        // Transaction mappings
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<CreateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        CreateMap<UpdateTransactionDto, Transaction>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        // Category mappings
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));

        CreateMap<CreateCategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        CreateMap<UpdateCategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Transactions, opt => opt.Ignore())
            .ForMember(dest => dest.Budgets, opt => opt.Ignore());

        // Budget mappings
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null));

        CreateMap<CreateBudgetDto, Budget>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Spent, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());

        CreateMap<UpdateBudgetDto, Budget>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Spent, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Account, opt => opt.Ignore());
    }
}