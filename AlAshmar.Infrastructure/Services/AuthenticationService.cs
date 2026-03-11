using AlAshmar.Application.Interfaces;
using AlAshmar.Application.Repos;
using AlAshmar.Application.UseCases.Managers.CreateManager;
using AlAshmar.Application.UseCases.Students.CreateStudent;
using AlAshmar.Application.UseCases.Teachers.CreateTeacher;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AlAshmar.Infrastructure.Services;

public class AuthenticationService(
    IRepositoryBase<User, Guid> userRepository,
    ITokenService tokenService) : IAuthenticationService
{
    public async Task<Result<AuthResult>> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetAsync(
            u => u.UserName == username,
            transform: q => q.Include(u => u.Role)
        );
        string hashedPs = PasswordHasher.Hash(password);

        if (user.IsError || user.Value == null)
            return new Error("401", "Invalid username or password", ErrorKind.Unauthorized);

        if (!PasswordHasher.Verify(password, user.Value.HashedPassword))
            return new Error("401", "Invalid username or password", ErrorKind.Unauthorized);

        var token = await tokenService.GenerateTokenAsync(user.Value.UserName, user.Value.Id, user.Value.RoleId, cancellationToken);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResult(token, expiresAt);
    }

    public async Task<Result<AuthResult>> RegisterManagerAsync(CreateManagerCommand command, CancellationToken cancellationToken = default)
    {
        // Check if user already exists
        var existingUser = await userRepository.GetAsync(u => u.UserName == command.UserName);
        if (!existingUser.IsError && existingUser.Value != null)
            return new Error("400", "Username already exists", ErrorKind.Validation);

        var user = new User
        {
            UserName = command.UserName,
            HashedPassword = PasswordHasher.Hash(command.Password),
            RoleId = Constants.DefaultManagerRoleId
        };

        var result = await userRepository.AddAsync(user);
        if (result.IsError)
            return result.Errors;

        var token = await tokenService.GenerateTokenAsync(user.UserName, user.Id, user.RoleId, cancellationToken);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResult(token, expiresAt);
    }

    public async Task<Result<AuthResult>> RegisterStudentAsync(CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        // Check if user already exists
        var existingUser = await userRepository.GetAsync(u => u.UserName == command.UserName);
        if (!existingUser.IsError && existingUser.Value != null)
            return new Error("400", "Username already exists", ErrorKind.Validation);

        var user = User.Create(
         command.UserName,
         PasswordHasher.Hash(command.Password),
         Constants.DefaultStudentRoleId
        );

        var result = await userRepository.AddAsync(user);
        if (result.IsError)
            return result.Errors;

        var token = await tokenService.GenerateTokenAsync(user.UserName, user.Id, user.RoleId, cancellationToken);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResult(token, expiresAt);
    }

    public async Task<Result<AuthResult>> RegisterTeacherAsync(CreateTeacherCommand command, CancellationToken cancellationToken = default)
    {
        // Check if user already exists
        var existingUser = await userRepository.GetAsync(u => u.UserName == command.UserName);
        if (!existingUser.IsError && existingUser.Value != null)
            return new Error("400", "Username already exists", ErrorKind.Validation);

        var user = new User
        {
            UserName = command.UserName,
            HashedPassword = PasswordHasher.Hash(command.Password),
            RoleId = Constants.DefaultTeacherRoleId
        };

        var result = await userRepository.AddAsync(user);
        if (result.IsError)
            return result.Errors;

        var token = await tokenService.GenerateTokenAsync(user.UserName, user.Id, user.RoleId, cancellationToken);
        var expiresAt = DateTime.UtcNow.AddHours(24);

        return new AuthResult(token, expiresAt);
    }
}
