using MediatR;
using SegundoCerebro.Domain.Interfaces;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.DeleteTodoItem;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTodoItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Id);

        if (todoItem == null)
            return false;

        await _unitOfWork.TodoItems.DeleteAsync(todoItem);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}