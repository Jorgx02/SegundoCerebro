using AutoMapper;
using MediatR;
using SegundoCerebro.Application.DTOs;
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
        var existingTodoItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Id);
        if (existingTodoItem == null)
            throw new KeyNotFoundException($"TodoItem with ID {request.Id} not found");

        // Lógica de completado automático
        if (existingTodoItem.Status != TodoItemStatus.Completed && request.TodoItem.Status == TodoItemStatus.Completed)
            existingTodoItem.CompletedAt = DateTime.UtcNow;
        else if (request.TodoItem.Status != TodoItemStatus.Completed)
            existingTodoItem.CompletedAt = null;

        _mapper.Map(request.TodoItem, existingTodoItem);
        existingTodoItem.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.TodoItems.UpdateAsync(existingTodoItem);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TodoItemDto>(existingTodoItem);
    }
}