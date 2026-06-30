using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.Habits.Queries.GetAllHabits;

/// <summary>
/// Manejador para la consulta que obtiene todos los hábitos.
/// </summary>
public class GetAllHabitsQueryHandler : IRequestHandler<GetAllHabitsQuery, IEnumerable<HabitDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetAllHabitsQueryHandler"/>.
    /// </summary>
    public GetAllHabitsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todos los hábitos.
    /// </summary>
    /// <param name="request">La consulta para obtener todos los hábitos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de hábitos.</returns>
    public async Task<IEnumerable<HabitDto>> Handle(GetAllHabitsQuery request, CancellationToken cancellationToken)
    {
        var habits = await _unitOfWork.Habits.GetAllAsync();
        return _mapper.Map<IEnumerable<HabitDto>>(habits);
    }
}