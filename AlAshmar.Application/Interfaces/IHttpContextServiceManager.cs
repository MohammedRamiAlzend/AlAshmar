namespace AlAshmar.Application.Interfaces;

/// <summary>
/// Service for accessing HTTP context information.
/// </summary>
public interface IHttpContextServiceManager
{
    Guid? GetCurrentUserId();
    string? GetCurrentUserName();
    bool IsAuthenticated();
}
