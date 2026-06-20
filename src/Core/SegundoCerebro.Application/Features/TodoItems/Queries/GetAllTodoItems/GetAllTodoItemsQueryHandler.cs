using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TodoItems.Queries.GetAllTodoItems;

/// <summary>
/// Manejador para la consulta que obtiene todas las tareas del usuario.
/// </summary>
public class GetAllTodoItemsQueryHandler : IRequestHandler<GetAllTodoItemsQuery, IEnumerable<TodoItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTodoItemsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Procesa la solicitud para obtener todas las tareas, incluyendo la información de sus proyectos asociados.
    /// </summary>
    /// <param name="request">La consulta.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una colección de DTOs de las tareas.</returns>
    public async Task<IEnumerable<TodoItemDto>> Handle(GetAllTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var todoItems = await _unitOfWork.TodoItems.GetTodoItemsWithProjectsAsync();
        return _mapper.Map<IEnumerable<TodoItemDto>>(todoItems);
    }
}