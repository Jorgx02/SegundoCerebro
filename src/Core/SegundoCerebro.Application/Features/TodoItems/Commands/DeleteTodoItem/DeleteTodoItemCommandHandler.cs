using MediatR;
using SegundoCerebro.Application.Exceptions;
using SegundoCerebro.Domain.Entities;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTodoItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Id);
        if (todoItem is null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        await _unitOfWork.TodoItems.DeleteAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();
    }
}