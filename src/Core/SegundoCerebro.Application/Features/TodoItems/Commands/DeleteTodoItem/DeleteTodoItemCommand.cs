using MediatR;

namespace SegundoCerebro.Application.Features.TodoItems.Commands.DeleteTodoItem;

public record DeleteTodoItemCommand(Guid Id) : IRequest;