using AutoMapper;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;

namespace SegundoCerebro.Application.Mappings;

/// <summary>
/// Configuración de los mapeos de Budget.
/// </summary>
public class BudgetMappingProfile : Profile
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="BudgetMappingProfile"/>.
    /// </summary>
    public BudgetMappingProfile()
    {
        // Budget mappings
        // Mapeo de Entidad a DTO para consultas.
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account != null ? src.Account.Name : null));

        // Mapeo de DTO de creación a Entidad.
        CreateMap<CreateBudgetDto, Budget>();

        // Mapeo de DTO de actualización a Entidad.
        CreateMap<UpdateBudgetDto, Budget>();
    }
}