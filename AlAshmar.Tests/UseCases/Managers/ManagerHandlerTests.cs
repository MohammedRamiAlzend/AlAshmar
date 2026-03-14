using AlAshmar.Application.DTOs;
using AlAshmar.Application.UseCases.Managers.CreateManager;
using AlAshmar.Application.UseCases.Managers.DeleteManager;
using AlAshmar.Application.UseCases.Managers.AddAttachment;
using AlAshmar.Application.UseCases.Managers.GetAllManagers;
using AlAshmar.Application.UseCases.Managers.GetAttachments;
using AlAshmar.Application.UseCases.Managers.GetManagerById;
using AlAshmar.Application.UseCases.Managers.UpdateManager;
using AlAshmar.Domain.Entities.Common;
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
            .ReturnsAsync(ApplicationErrors.DatabaseError);

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
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.ManagerNotFound);

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
            .ReturnsAsync(ApplicationErrors.DatabaseError);

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
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.ManagerNotFound);

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
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.ManagerNotFound);

        var handler = new UpdateManagerHandler(_repoMock.Object);
        var command = new UpdateManagerCommand(Guid.NewGuid(), "New Name");
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class AddManagerAttachmentHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _managerRepoMock = new();
    private readonly Mock<IRepositoryBase<Attacment, Guid>> _attachmentRepoMock = new();

    [Fact]
    public async Task Handle_ExistingManager_AddsAttachmentAndReturnsSuccess()
    {
        var managerId = Guid.NewGuid();
        var manager = new Manager
        {
            Id = managerId,
            Name = "Manager",
            ManagerAttachments = new List<ManagerAttachment>()
        };

        _managerRepoMock.Setup(r => r.GetByIdAsync(managerId)).ReturnsAsync(manager);
        _attachmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Attacment>())).ReturnsAsync(new Success());
        _managerRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Manager>())).ReturnsAsync(new Updated());

        var handler = new AddManagerAttachmentHandler(_managerRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddManagerAttachmentCommand(managerId, "/path/to/file.pdf", "application/pdf", "safe.pdf", "original.pdf", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        _attachmentRepoMock.Verify(r => r.AddAsync(It.IsAny<Attacment>()), Times.Once);
        _managerRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Manager>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingManager_ReturnsNotFoundError()
    {
        _managerRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.ManagerNotFound);

        var handler = new AddManagerAttachmentHandler(_managerRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddManagerAttachmentCommand(Guid.NewGuid(), "/path/file.pdf", "application/pdf", "safe.pdf", "orig.pdf", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetManagerAttachmentsHandlerTests
{
    private readonly Mock<IRepositoryBase<Manager, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingManager_ReturnsAttachments()
    {
        var managerId = Guid.NewGuid();
        var manager = new Manager
        {
            Id = managerId,
            Name = "Manager",
            ManagerAttachments = new List<ManagerAttachment>
            {
                new() { ManagerId = managerId, AttachmentId = Guid.NewGuid(),
                    Attachment = new Attacment { Id = Guid.NewGuid(), Path = "/file.pdf", Type = "application/pdf", SafeName = "safe.pdf", OriginalName = "orig.pdf" } }
            }
        };

        _repoMock.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Manager, bool>>?>(),
                It.IsAny<Func<IQueryable<Manager>, IQueryable<Manager>>?>()))
            .ReturnsAsync(manager);

        var handler = new GetManagerAttachmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetManagerAttachmentsQuery(managerId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_NonExistingManager_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Manager, bool>>?>(),
                It.IsAny<Func<IQueryable<Manager>, IQueryable<Manager>>?>()))
            .ReturnsAsync(ApplicationErrors.ManagerNotFound);

        var handler = new GetManagerAttachmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetManagerAttachmentsQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}
