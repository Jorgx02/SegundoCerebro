using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Enums;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTodoItemCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = _mapper.Map<TodoItem>(request.TodoItem);
        todoItem.Id = Guid.NewGuid();
        todoItem.CreatedAt = DateTime.UtcNow;
        todoItem.Status = TodoItemStatus.Inbox; // Todas las tareas nuevas van al Inbox por defecto

        var createdTodoItem = await _unitOfWork.TodoItems.AddAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TodoItemDto>(createdTodoItem);
    }
}