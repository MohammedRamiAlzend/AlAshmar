namespace AlAshmar.Application.Interfaces;

public interface IHttpContextServiceManager
{
    Guid? GetCurrentUserId();
    string? GetCurrentUserName();
    bool IsAuthenticated();
}
