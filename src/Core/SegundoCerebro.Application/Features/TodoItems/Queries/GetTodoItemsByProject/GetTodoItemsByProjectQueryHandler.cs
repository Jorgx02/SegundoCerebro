using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TodoItems.Queries.GetTodoItemsByProject;

public class GetTodoItemsByProjectQueryHandler : IRequestHandler<GetTodoItemsByProjectQuery, IEnumerable<TodoItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTodoItemsByProjectQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItemDto>> Handle(GetTodoItemsByProjectQuery request, CancellationToken cancellationToken)
    {
        var projectExists = await _unitOfWork.Projects.ExistsAsync(request.ProjectId);
        if (!projectExists)
        {
            throw new NotFoundException(nameof(Project), request.ProjectId);
        }

        var todoItems = await _unitOfWork.TodoItems.GetByProjectIdAsync(request.ProjectId);

        return _mapper.Map<IEnumerable<TodoItemDto>>(todoItems);
    }
}