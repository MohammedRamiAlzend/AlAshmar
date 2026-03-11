using AlAshmar.Application.UseCases.Managers.CreateManager;
using AlAshmar.Application.UseCases.Students.CreateStudent;
using AlAshmar.Application.UseCases.Teachers.CreateTeacher;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.Interfaces;

/// <summary>
/// Service for handling authentication operations.
/// </summary>
public interface IAuthenticationService
{
    Task<Result<AuthResult>> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResult>> RegisterManagerAsync(CreateManagerCommand request, CancellationToken cancellationToken = default);
    Task<Result<AuthResult>> RegisterTeacherAsync(CreateTeacherCommand createTeacherCommand, CancellationToken cancellationToken = default);
    Task<Result<AuthResult>> RegisterStudentAsync(CreateStudentCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of an authentication operation.
/// </summary>
public record AuthResult(string Token, DateTime ExpiresAt);
