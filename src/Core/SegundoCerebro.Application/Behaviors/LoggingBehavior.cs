using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace SegundoCerebro.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Iniciando Request: {RequestName}", requestName);

        var timer = new Stopwatch();
        timer.Start();

        try
        {
            var response = await next();
            timer.Stop();

            _logger.LogInformation("Completado Request: {RequestName} en {ElapsedMilliseconds}ms", requestName, timer.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError(ex, "Error en Request: {RequestName} después de {ElapsedMilliseconds}ms. Mensaje: {ErrorMessage}", requestName, timer.ElapsedMilliseconds, ex.Message);
            throw;
        }
    }
}