using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, TodoItemDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateTodoItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TodoItemDto> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItemToUpdate = await _unitOfWork.TodoItems.GetByIdAsync(request.Id);
        if (todoItemToUpdate is null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        // Lógica de negocio: Gestionar la fecha de completado.
        if (request.TodoItemDto.Status == TodoItemStatus.Completed && todoItemToUpdate.Status != TodoItemStatus.Completed)
        {
            todoItemToUpdate.CompletedAt = DateTime.UtcNow;
        }
        else if (request.TodoItemDto.Status != TodoItemStatus.Completed)
        {
            todoItemToUpdate.CompletedAt = null;
        }

        _mapper.Map(request.TodoItemDto, todoItemToUpdate);
        todoItemToUpdate.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TodoItemDto>(todoItemToUpdate);
    }
}
