using AlAshmar.Application.DTOs.Authorization;
using AlAshmar.Application.Interfaces;
using AlAshmar.Application.UseCases.Authorization.AssignPermissionsToRole;
using AlAshmar.Application.UseCases.Authorization.AssignRoleToUser;
using AlAshmar.Application.UseCases.Authorization.GetAllPermissions;
using AlAshmar.Application.UseCases.Authorization.GetAllRoles;
using AlAshmar.Application.UseCases.Authorization.GetRoleById;

namespace AlAshmar.Tests.UseCases.Authorization;

public class AssignPermissionsToRoleHandlerTests
{
    private readonly Mock<IAuthorizationService> _authServiceMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsUpdatedRole()
    {
        var roleId = Guid.NewGuid();
        var permissionIds = new List<Guid> { Guid.NewGuid() };
        var dto = new AssignPermissionsToRoleDto(roleId, permissionIds);
        var expectedRole = new RoleDto(roleId, "Student", new List<PermissionDto>());

        _authServiceMock.Setup(s => s.AssignPermissionsToRoleAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRole);

        var handler = new AssignPermissionsToRoleHandler(_authServiceMock.Object);
        var result = await handler.Handle(new AssignPermissionsToRoleCommand(dto), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(roleId, result.Value!.Id);
    }

    [Fact]
    public async Task Handle_ServiceFailure_ReturnsError()
    {
        var dto = new AssignPermissionsToRoleDto(Guid.NewGuid(), new List<Guid>());

        _authServiceMock.Setup(s => s.AssignPermissionsToRoleAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApplicationErrors.RoleNotFound);

        var handler = new AssignPermissionsToRoleHandler(_authServiceMock.Object);
        var result = await handler.Handle(new AssignPermissionsToRoleCommand(dto), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class AssignRoleToUserHandlerTests
{
    private readonly Mock<IAuthorizationService> _authServiceMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var dto = new AssignRoleToUserDto(Guid.NewGuid(), Guid.NewGuid());

        _authServiceMock.Setup(s => s.AssignRoleToUserAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Success());

        var handler = new AssignRoleToUserHandler(_authServiceMock.Object);
        var result = await handler.Handle(new AssignRoleToUserCommand(dto), CancellationToken.None);

        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Handle_ServiceFailure_ReturnsError()
    {
        var dto = new AssignRoleToUserDto(Guid.NewGuid(), Guid.NewGuid());

        _authServiceMock.Setup(s => s.AssignRoleToUserAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApplicationErrors.UserNotFound);

        var handler = new AssignRoleToUserHandler(_authServiceMock.Object);
        var result = await handler.Handle(new AssignRoleToUserCommand(dto), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetAllPermissionsHandlerTests
{
    private readonly Mock<IAuthorizationService> _authServiceMock = new();

    [Fact]
    public async Task Handle_ReturnsAllPermissions()
    {
        var permissions = new List<PermissionDto>
        {
            new(Guid.NewGuid(), "Read", "Read access", "Students", "Read"),
            new(Guid.NewGuid(), "Write", "Write access", "Students", "Write")
        };

        _authServiceMock.Setup(s => s.GetAllPermissionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(permissions);

        var handler = new GetAllPermissionsHandler(_authServiceMock.Object);
        var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task Handle_ServiceFailure_ReturnsError()
    {
        _authServiceMock.Setup(s => s.GetAllPermissionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new GetAllPermissionsHandler(_authServiceMock.Object);
        var result = await handler.Handle(new GetAllPermissionsQuery(), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetAllRolesHandlerTests
{
    private readonly Mock<IAuthorizationService> _authServiceMock = new();

    [Fact]
    public async Task Handle_ReturnsAllRoles()
    {
        var roles = new List<RoleDto>
        {
            new(Guid.NewGuid(), "Student", new List<PermissionDto>()),
            new(Guid.NewGuid(), "Teacher", new List<PermissionDto>()),
            new(Guid.NewGuid(), "Manager", new List<PermissionDto>())
        };

        _authServiceMock.Setup(s => s.GetAllRolesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(roles);

        var handler = new GetAllRolesHandler(_authServiceMock.Object);
        var result = await handler.Handle(new GetAllRolesQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(3, result.Value!.Count);
    }

    [Fact]
    public async Task Handle_ServiceFailure_ReturnsError()
    {
        _authServiceMock.Setup(s => s.GetAllRolesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new GetAllRolesHandler(_authServiceMock.Object);
        var result = await handler.Handle(new GetAllRolesQuery(), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetRoleByIdHandlerTests
{
    private readonly Mock<IAuthorizationService> _authServiceMock = new();

    [Fact]
    public async Task Handle_ExistingRole_ReturnsRoleDto()
    {
        var roleId = Guid.NewGuid();
        var role = new RoleDto(roleId, "Student", new List<PermissionDto>());

        _authServiceMock.Setup(s => s.GetRoleByIdAsync(roleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(role);

        var handler = new GetRoleByIdHandler(_authServiceMock.Object);
        var result = await handler.Handle(new GetRoleByIdQuery(roleId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(roleId, result.Value!.Id);
        Assert.Equal("Student", result.Value.Type);
    }

    [Fact]
    public async Task Handle_NonExistingRole_ReturnsNotFoundError()
    {
        _authServiceMock.Setup(s => s.GetRoleByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApplicationErrors.RoleNotFound);

        var handler = new GetRoleByIdHandler(_authServiceMock.Object);
        var result = await handler.Handle(new GetRoleByIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}
