using AlAshmar.Application.DTOs;
using AlAshmar.Application.UseCases.Managers.CreateManager;
using AlAshmar.Application.UseCases.Managers.DeleteManager;
using AlAshmar.Application.UseCases.Managers.GetAllManagers;
using AlAshmar.Application.UseCases.Managers.GetManagerById;
using AlAshmar.Application.UseCases.Managers.UpdateManager;
using AlAshmar.Domain.Entities.Managers;

namespace AlAshmar.Tests.UseCases.Managers;

public class CreateManagerHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsManagerDto()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Manager>()))
            .ReturnsAsync(new Success());

        var handler = new CreateManagerHandler(_repoMock.Object);
        var command = new CreateManagerCommand("Manager Name", "manager_user", "Secure@789");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal("Manager Name", result.Value.Name);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Manager>()))
            .ReturnsAsync(new Error("DB_ERROR", "Database failure"));

        var handler = new CreateManagerHandler(_repoMock.Object);
        var command = new CreateManagerCommand("Manager Name", "manager_user", "Secure@789");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class DeleteManagerHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingManager_ReturnsSuccess()
    {
        var managerId = Guid.NewGuid();
        var manager = new Manager { Id = managerId, Name = "Test Manager" };

        _repoMock.Setup(r => r.GetByIdAsync(managerId)).ReturnsAsync(manager);
        _repoMock.Setup(r => r.RemoveAsync(It.IsAny<Expression<Func<Manager, bool>>>()))
            .ReturnsAsync(new Deleted());

        var handler = new DeleteManagerHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteManagerCommand(managerId), CancellationToken.None);

        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Handle_NonExistingManager_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Error("NOT_FOUND", "Manager not found", ErrorKind.NotFound));

        var handler = new DeleteManagerHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteManagerCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetAllManagersHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ReturnsAllManagers()
    {
        var managers = new List<Manager>
        {
            new() { Id = Guid.NewGuid(), Name = "Manager One" },
            new() { Id = Guid.NewGuid(), Name = "Manager Two" }
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Manager, bool>>?>(),
                It.IsAny<Func<IQueryable<Manager>, IQueryable<Manager>>?>(),
                It.IsAny<Func<IQueryable<Manager>, IOrderedQueryable<Manager>>?>()))
            .ReturnsAsync(managers);

        var handler = new GetAllManagersHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllManagersQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Manager, bool>>?>(),
                It.IsAny<Func<IQueryable<Manager>, IQueryable<Manager>>?>(),
                It.IsAny<Func<IQueryable<Manager>, IOrderedQueryable<Manager>>?>()))
            .ReturnsAsync(new Error("DB_ERROR", "Database error"));

        var handler = new GetAllManagersHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllManagersQuery(), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetManagerByIdHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingManager_ReturnsManagerDto()
    {
        var managerId = Guid.NewGuid();
        var manager = new Manager { Id = managerId, Name = "Test Manager" };

        _repoMock.Setup(r => r.GetByIdAsync(managerId)).ReturnsAsync(manager);

        var handler = new GetManagerByIdHandler(_repoMock.Object);
        var result = await handler.Handle(new GetManagerByIdQuery(managerId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(managerId, result.Value!.Id);
        Assert.Equal("Test Manager", result.Value.Name);
    }

    [Fact]
    public async Task Handle_NonExistingManager_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Error("NOT_FOUND", "Manager not found", ErrorKind.NotFound));

        var handler = new GetManagerByIdHandler(_repoMock.Object);
        var result = await handler.Handle(new GetManagerByIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class UpdateManagerHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingManager_UpdatesAndReturnsManagerDto()
    {
        var managerId = Guid.NewGuid();
        var manager = new Manager { Id = managerId, Name = "Old Name" };

        _repoMock.Setup(r => r.GetByIdAsync(managerId)).ReturnsAsync(manager);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Manager>())).ReturnsAsync(new Updated());

        var handler = new UpdateManagerHandler(_repoMock.Object);
        var command = new UpdateManagerCommand(managerId, "New Name");
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal("New Name", result.Value!.Name);
    }

    [Fact]
    public async Task Handle_NonExistingManager_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Error("NOT_FOUND", "Manager not found", ErrorKind.NotFound));

        var handler = new UpdateManagerHandler(_repoMock.Object);
        var command = new UpdateManagerCommand(Guid.NewGuid(), "New Name");
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}
