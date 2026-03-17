using AlAshmar.Application.UseCases.Teachers.AddAttachment;
using AlAshmar.Application.UseCases.Teachers.CreateTeacher;
using AlAshmar.Application.UseCases.Teachers.DeleteTeacher;
using AlAshmar.Application.UseCases.Teachers.GetAllTeachers;
using AlAshmar.Application.UseCases.Teachers.GetAllTeachersFiltered;
using AlAshmar.Application.UseCases.Teachers.GetAttachments;
using AlAshmar.Application.UseCases.Teachers.GetTeacherById;
using AlAshmar.Application.UseCases.Teachers.UpdateTeacher;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Tests.UseCases.Teachers;

public class CreateTeacherHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsTeacherDto()
    {
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Teacher>()))
            .ReturnsAsync(new Success());

        var handler = new CreateTeacherHandler(_repoMock.Object);
        var command = new CreateTeacherCommand("Khalid", "Ahmad", "Sara", "9876543210", "khalid@test.com", "khalid_user", "Secure@456");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal("Khalid", result.Value.Name);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Teacher>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new CreateTeacherHandler(_repoMock.Object);
        var command = new CreateTeacherCommand("Khalid", "Ahmad", "Sara", "8888888881", null, "khalid_user", "Secure@456");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class DeleteTeacherHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingTeacher_ReturnsSuccess()
    {
        var teacherId = Guid.NewGuid();
        var teacher = Teacher.Create("Test", "F", "M", "9876543210", null, "teacher_user", "Pass@123");
        teacher.Id = teacherId;

        _repoMock.Setup(r => r.GetByIdAsync(teacherId)).ReturnsAsync(teacher);
        _repoMock.Setup(r => r.RemoveAsync(It.IsAny<Expression<Func<Teacher, bool>>>()))
            .ReturnsAsync(new Deleted());

        var handler = new DeleteTeacherHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteTeacherCommand(teacherId), CancellationToken.None);

        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Handle_NonExistingTeacher_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.TeacherNotFound);

        var handler = new DeleteTeacherHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteTeacherCommand(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetAllTeachersHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ReturnsAllTeachers()
    {
        var teachers = new List<Teacher>
        {
            Teacher.Create("Khalid", "Ahmad", "Sara", "9876543210", null, "user1", "Pass@123"),
            Teacher.Create("Noor", "Hassan", "Layla", "1234567890", null, "user2", "Pass@123")
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()))
            .ReturnsAsync(teachers);

        var handler = new GetAllTeachersHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllTeachersQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new GetAllTeachersHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllTeachersQuery(), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetAllTeachersFilteredHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_NoFilters_ReturnsAllTeachers()
    {
        var teachers = new List<Teacher>
        {
            Teacher.Create("Khalid", "Ahmad", "Sara", "9876543210", null, "user1", "Pass@123")
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()))
            .ReturnsAsync(teachers);

        var handler = new GetAllTeachersFilteredHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllTeachersFilteredQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_WithPagination_UsesPagedRepository()
    {
        var pagedList = new PagedList<Teacher>(new List<Teacher>(), 0, 1, 10);

        _repoMock.Setup(r => r.GetPagedAsync(
                1, 10,
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()))
            .ReturnsAsync(pagedList);

        var handler = new GetAllTeachersFilteredHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllTeachersFilteredQuery(PageNumber: 1, PageSize: 10), CancellationToken.None);

        Assert.False(result.IsError);
        _repoMock.Verify(r => r.GetPagedAsync(1, 10,
            It.IsAny<Expression<Func<Teacher, bool>>?>(),
            It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
            It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()), Times.Once);
    }
}

public class GetTeacherByIdHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingTeacher_ReturnsTeacherDto()
    {
        var teacherId = Guid.NewGuid();
        var teacher = Teacher.Create("Khalid", "Ahmad", "Sara", "9876543210", null, "user1", "Pass@123");
        teacher.Id = teacherId;

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()))
            .ReturnsAsync(new List<Teacher> { teacher });

        var handler = new GetTeacherByIdHandler(_repoMock.Object);
        var result = await handler.Handle(new GetTeacherByIdQuery(teacherId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(teacherId, result.Value!.Id);
        Assert.Equal("Khalid", result.Value.Name);
    }

    [Fact]
    public async Task Handle_NonExistingTeacher_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IOrderedQueryable<Teacher>>?>()))
            .ReturnsAsync(new List<Teacher>());

        var handler = new GetTeacherByIdHandler(_repoMock.Object);
        var result = await handler.Handle(new GetTeacherByIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class UpdateTeacherHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingTeacher_UpdatesAndReturnsTeacherDto()
    {
        var teacherId = Guid.NewGuid();
        var teacher = Teacher.Create("Old Name", "Ahmad", "Sara", "9876543210", null, "user1", "Pass@123");
        teacher.Id = teacherId;

        _repoMock.Setup(r => r.GetByIdAsync(teacherId)).ReturnsAsync(teacher);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Teacher, bool>>>(), It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Teacher>())).ReturnsAsync(new Updated());

        var handler = new UpdateTeacherHandler(_repoMock.Object);
        var command = new UpdateTeacherCommand(teacherId, "New Name", "Ahmad", "Sara", "8888888882", null);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal("New Name", result.Value!.Name);
    }

    [Fact]
    public async Task Handle_NonExistingTeacher_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.TeacherNotFound);

        var handler = new UpdateTeacherHandler(_repoMock.Object);
        var command = new UpdateTeacherCommand(Guid.NewGuid(), "Name", "Father", "Mother", "8888888883", null);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class AddTeacherAttachmentHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _teacherRepoMock = new();
    private readonly Mock<IRepositoryBase<Attachment, Guid>> _attachmentRepoMock = new();

    [Fact]
    public async Task Handle_ExistingTeacher_AddsAttachmentAndReturnsSuccess()
    {
        var teacherId = Guid.NewGuid();
        var teacher = Teacher.Create("Khalid", "Ahmad", "Sara", "9876543210", null, "user1", "Pass@123");
        teacher.Id = teacherId;

        _teacherRepoMock.Setup(r => r.GetByIdAsync(teacherId)).ReturnsAsync(teacher);
        _attachmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Attachment>())).ReturnsAsync(new Success());
        _teacherRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Teacher>())).ReturnsAsync(new Updated());

        var handler = new AddTeacherAttachmentHandler(_teacherRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddTeacherAttachmentCommand(teacherId, "/path/to/file.pdf", "application/pdf", "safe.pdf", "original.pdf", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        _attachmentRepoMock.Verify(r => r.AddAsync(It.IsAny<Attachment>()), Times.Once);
        _teacherRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Teacher>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTeacher_ReturnsNotFoundError()
    {
        _teacherRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(ApplicationErrors.TeacherNotFound);

        var handler = new AddTeacherAttachmentHandler(_teacherRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddTeacherAttachmentCommand(Guid.NewGuid(), "/path/file.pdf", "application/pdf", "safe.pdf", "orig.pdf", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetTeacherAttachmentsHandlerTests
{
    private readonly Mock<IRepositoryBase<Teacher, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingTeacher_ReturnsAttachments()
    {
        var teacherId = Guid.NewGuid();
        var teacher = Teacher.Create("Khalid", "Ahmad", "Sara", "9876543210", null, "user1", "Pass@123");
        teacher.Id = teacherId;
        teacher.TeacherAttachments.Add(new TeacherAttachment
        {
            TeacherId = teacherId,
            AttachmentId = Guid.NewGuid(),
            Attachment = new Attachment { Id = Guid.NewGuid(), Path = "/file.pdf", Type = "application/pdf", SafeName = "safe.pdf", OriginalName = "orig.pdf" }
        });

        _repoMock.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>()))
            .ReturnsAsync(teacher);

        var handler = new GetTeacherAttachmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetTeacherAttachmentsQuery(teacherId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_NonExistingTeacher_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Teacher, bool>>?>(),
                It.IsAny<Func<IQueryable<Teacher>, IQueryable<Teacher>>?>()))
            .ReturnsAsync(ApplicationErrors.TeacherNotFound);

        var handler = new GetTeacherAttachmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetTeacherAttachmentsQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}
