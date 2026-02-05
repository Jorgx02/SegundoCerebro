using AutoMapper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Application.Mappings;

public class BudgetMappingProfile : Profile
{
    public BudgetMappingProfile()
    {
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null));

        CreateMap<CreateBudgetDto, Budget>();
        CreateMap<UpdateBudgetDto, Budget>();
    }
}