using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.UseCases.Students.CreateStudent;
using AlAshmar.Application.UseCases.Students.DeleteStudent;
using AlAshmar.Application.UseCases.Students.GetAllStudents;
using AlAshmar.Application.UseCases.Students.GetAllStudentsFiltered;
using AlAshmar.Application.UseCases.Students.GetStudentById;
using AlAshmar.Application.UseCases.Students.UpdateStudent;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Tests.UseCases.Students;

public class CreateStudentHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ReturnsStudentBasicInfoDto()
    {
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Student>()))
            .ReturnsAsync(new Success());

        var handler = new CreateStudentHandler(_repoMock.Object);
        var command = new CreateStudentCommand("Ahmed", "Ali", "Fatima", "1234567890", "ahmed@test.com", "ahmed_user", "Secure@123");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal("Ahmed", result.Value.Name);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        var error = new Error("DB_ERROR", "Database failure", ErrorKind.Failure);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Student>()))
            .ReturnsAsync(error);

        var handler = new CreateStudentHandler(_repoMock.Object);
        var command = new CreateStudentCommand("Ahmed", "Ali", "Fatima", "9999999990", null, "ahmed_user", "Secure@123");

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class DeleteStudentHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingStudent_ReturnsSuccess()
    {
        var studentId = Guid.NewGuid();
        var student = new Student { Id = studentId, Name = "Test", FatherName = "F", MotherName = "M" };

        _repoMock.Setup(r => r.GetByIdAsync(studentId))
            .ReturnsAsync(student);
        _repoMock.Setup(r => r.RemoveAsync(It.IsAny<Expression<Func<Student, bool>>>()))
            .ReturnsAsync(new Deleted());

        var handler = new DeleteStudentHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteStudentCommand(studentId), CancellationToken.None);

        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Handle_NonExistingStudent_ReturnsNotFoundError()
    {
        var studentId = Guid.NewGuid();

        _repoMock.Setup(r => r.GetByIdAsync(studentId))
            .ReturnsAsync(new Error("NOT_FOUND", "Student not found", ErrorKind.NotFound));

        var handler = new DeleteStudentHandler(_repoMock.Object);
        var result = await handler.Handle(new DeleteStudentCommand(studentId), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetAllStudentsHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ReturnsAllStudents()
    {
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), Name = "Ahmed", FatherName = "Ali", MotherName = "Fatima" },
            new() { Id = Guid.NewGuid(), Name = "Sara", FatherName = "Omar", MotherName = "Hana" }
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
                It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()))
            .ReturnsAsync(students);

        var handler = new GetAllStudentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllStudentsQuery(), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
                It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()))
            .ReturnsAsync(new Error("DB_ERROR", "Database error"));

        var handler = new GetAllStudentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAllStudentsQuery(), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetAllStudentsFilteredHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_NoFilters_ReturnsAllStudents()
    {
        var students = new List<Student>
        {
            new() { Id = Guid.NewGuid(), Name = "Ahmed", FatherName = "Ali", MotherName = "Fatima",
                    StudentClassEventsPoints = new List<StudentClassEventsPoint>(),
                    StudentHadiths = new List<StudentHadith>(),
                    StudentQuraanPages = new List<StudentQuraanPage>(),
                    Points = new List<Domain.Entities.Academic.Point>() }
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
                It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()))
            .ReturnsAsync(students);

        var handler = new GetAllStudentsFilteredHandler(_repoMock.Object);
        var query = new GetAllStudentsFilteredQuery();
        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_WithPagination_UsesPagedRepository()
    {
        var pagedList = new PagedList<Student>(new List<Student>(), 0, 1, 10);

        _repoMock.Setup(r => r.GetPagedAsync(
                1, 10,
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
                It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()))
            .ReturnsAsync(pagedList);

        var handler = new GetAllStudentsFilteredHandler(_repoMock.Object);
        var query = new GetAllStudentsFilteredQuery(PageNumber: 1, PageSize: 10);
        var result = await handler.Handle(query, CancellationToken.None);

        Assert.False(result.IsError);
        _repoMock.Verify(r => r.GetPagedAsync(1, 10,
            It.IsAny<Expression<Func<Student, bool>>?>(),
            It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
            It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()), Times.Once);
    }
}

public class GetStudentByIdHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingStudent_ReturnsStudentDetail()
    {
        var studentId = Guid.NewGuid();
        var student = new Student
        {
            Id = studentId,
            Name = "Ahmed",
            FatherName = "Ali",
            MotherName = "Fatima",
            StudentContactInfos = new List<StudentContactInfo>(),
            StudentAttachments = new List<StudentAttachment>(),
            StudentHadiths = new List<StudentHadith>(),
            StudentQuraanPages = new List<StudentQuraanPage>(),
            StudentClassEventsPoints = new List<StudentClassEventsPoint>(),
            Points = new List<Domain.Entities.Academic.Point>()
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
                It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()))
            .ReturnsAsync(new List<Student> { student });

        var handler = new GetStudentByIdHandler(_repoMock.Object);
        var result = await handler.Handle(new GetStudentByIdQuery(studentId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(studentId, result.Value!.Id);
        Assert.Equal("Ahmed", result.Value.Name);
    }

    [Fact]
    public async Task Handle_NonExistingStudent_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>(),
                It.IsAny<Func<IQueryable<Student>, IOrderedQueryable<Student>>?>()))
            .ReturnsAsync(new List<Student>());

        var handler = new GetStudentByIdHandler(_repoMock.Object);
        var result = await handler.Handle(new GetStudentByIdQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class UpdateStudentHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingStudent_UpdatesAndReturnsSuccess()
    {
        var studentId = Guid.NewGuid();
        var student = new Student { Id = studentId, Name = "Old Name", FatherName = "Ali", MotherName = "Fatima" };

        _repoMock.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(student);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Student, bool>>>(), It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>()))
            .ReturnsAsync(false);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Student>())).ReturnsAsync(new Updated());

        var handler = new UpdateStudentHandler(_repoMock.Object);
        var command = new UpdateStudentCommand(studentId, "New Name", "Ali", "Fatima", "9999999991", null);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Equal("Student updated successfully", result.Value.Data?.ToString());
    }

    [Fact]
    public async Task Handle_NonExistingStudent_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Error("NOT_FOUND", "Student not found", ErrorKind.NotFound));

        var handler = new UpdateStudentHandler(_repoMock.Object);
        var command = new UpdateStudentCommand(Guid.NewGuid(), "Name", "Father", "Mother", "9999999992", null);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}
