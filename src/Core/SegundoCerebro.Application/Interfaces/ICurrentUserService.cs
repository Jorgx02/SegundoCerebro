namespace SegundoCerebro.Application.Interfaces;

/// <summary>
/// Interfaz para obtener información del usuario actual.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
}