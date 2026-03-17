using AlAshmar.Application.UseCases.Students.AddAttachment;
using AlAshmar.Application.UseCases.Students.EnrollInClass;
using AlAshmar.Application.UseCases.Students.GetAttachments;
using AlAshmar.Application.UseCases.Students.GetAttendanceRecords;
using AlAshmar.Application.UseCases.Students.GetClassEnrollments;
using AlAshmar.Application.UseCases.Students.GetMemorizationProgress;
using AlAshmar.Application.UseCases.Students.GetPoints;
using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Tests.UseCases.Students;

public class AddAttachmentHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _studentRepoMock = new();
    private readonly Mock<IRepositoryBase<Attachment, Guid>> _attachmentRepoMock = new();

    [Fact]
    public async Task Handle_ExistingStudent_AddsAttachmentAndReturnsSuccess()
    {
        var studentId = Guid.NewGuid();
        var student = Student.Create("Ahmed", "Ali", "Fatima", "9876543210", null, "student_user", "Pass@123");
        student.Id = studentId;

        _studentRepoMock.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(student);
        _attachmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Attachment>())).ReturnsAsync(new Success());
        _studentRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Student>())).ReturnsAsync(new Updated());

        var handler = new AddAttachmentHandler(_studentRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddAttachmentCommand(studentId, "/path/to/file.jpg", "image/jpeg", "safe_name.jpg", "original.jpg", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsError);
        _attachmentRepoMock.Verify(r => r.AddAsync(It.IsAny<Attachment>()), Times.Once);
        _studentRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Student>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingStudent_ReturnsNotFoundError()
    {
        _studentRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(ApplicationErrors.StudentNotFound);

        var handler = new AddAttachmentHandler(_studentRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddAttachmentCommand(Guid.NewGuid(), "/path/file.jpg", "image/jpeg", "safe.jpg", "orig.jpg", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }

    [Fact]
    public async Task Handle_AttachmentRepositoryFailure_ReturnsError()
    {
        var studentId = Guid.NewGuid();
        var student = Student.Create("Ahmed", "Ali", "M", "9876543210", null, "student_user2", "Pass@123");
        student.Id = studentId;

        _studentRepoMock.Setup(r => r.GetByIdAsync(studentId)).ReturnsAsync(student);
        _attachmentRepoMock.Setup(r => r.AddAsync(It.IsAny<Attachment>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new AddAttachmentHandler(_studentRepoMock.Object, _attachmentRepoMock.Object);
        var command = new AddAttachmentCommand(studentId, "/path/file.jpg", "image/jpeg", "safe.jpg", "orig.jpg", null);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class EnrollInClassHandlerTests
{
    private readonly Mock<IRepositoryBase<ClassStudentEnrollment, Guid>> _enrollmentRepoMock = new();

    [Fact]
    public async Task Handle_NewEnrollment_ReturnsSuccess()
    {
        var studentId = Guid.NewGuid();
        var classId = Guid.NewGuid();

        _enrollmentRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ClassStudentEnrollment, bool>>?>(),
                It.IsAny<Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>>?>(),
                It.IsAny<Func<IQueryable<ClassStudentEnrollment>, IOrderedQueryable<ClassStudentEnrollment>>?>()))
            .ReturnsAsync(new List<ClassStudentEnrollment>());

        _enrollmentRepoMock.Setup(r => r.AddAsync(It.IsAny<ClassStudentEnrollment>()))
            .ReturnsAsync(new Success());

        var handler = new EnrollInClassHandler(_enrollmentRepoMock.Object);
        var result = await handler.Handle(new EnrollInClassCommand(studentId, classId), CancellationToken.None);

        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Handle_AlreadyEnrolled_ReturnsConflictError()
    {
        var studentId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var existing = new ClassStudentEnrollment { StudentId = studentId, ClassId = classId };

        _enrollmentRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ClassStudentEnrollment, bool>>?>(),
                It.IsAny<Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>>?>(),
                It.IsAny<Func<IQueryable<ClassStudentEnrollment>, IOrderedQueryable<ClassStudentEnrollment>>?>()))
            .ReturnsAsync(new List<ClassStudentEnrollment> { existing });

        var handler = new EnrollInClassHandler(_enrollmentRepoMock.Object);
        var result = await handler.Handle(new EnrollInClassCommand(studentId, classId), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.Conflict, result.TopError.Type);
    }
}

public class GetAttachmentsHandlerTests
{
    private readonly Mock<IRepositoryBase<Student, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ExistingStudent_ReturnsAttachments()
    {
        var studentId = Guid.NewGuid();
        var student = Student.Create("Ahmed", "Ali", "Fatima", "9876543210", null, "student_user3", "Pass@123");
        student.Id = studentId;
        student.StudentAttachments.Add(new StudentAttachment
        {
            StudentId = studentId,
            AttachmentId = Guid.NewGuid(),
            Attachment = new Attachment { Id = Guid.NewGuid(), Path = "/file.jpg", Type = "image/jpeg", SafeName = "safe.jpg", OriginalName = "orig.jpg" }
        });

        _repoMock.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>()))
            .ReturnsAsync(student);

        var handler = new GetAttachmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAttachmentsQuery(studentId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_NonExistingStudent_ReturnsNotFoundError()
    {
        _repoMock.Setup(r => r.GetAsync(
                It.IsAny<Expression<Func<Student, bool>>?>(),
                It.IsAny<Func<IQueryable<Student>, IQueryable<Student>>?>()))
            .ReturnsAsync(ApplicationErrors.StudentNotFound);

        var handler = new GetAttachmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAttachmentsQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
        Assert.Equal(ErrorKind.NotFound, result.TopError.Type);
    }
}

public class GetAttendanceRecordsHandlerTests
{
    private readonly Mock<IRepositoryBase<StudentAttendance, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ReturnsAttendanceRecords()
    {
        var studentId = Guid.NewGuid();
        var attendances = new List<StudentAttendance>
        {
            new() { Id = Guid.NewGuid(), ClassStudentId = studentId, StartDate = DateTime.Today.AddDays(-1), EndDate = DateTime.Today }
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<StudentAttendance, bool>>?>(),
                It.IsAny<Func<IQueryable<StudentAttendance>, IQueryable<StudentAttendance>>?>(),
                It.IsAny<Func<IQueryable<StudentAttendance>, IOrderedQueryable<StudentAttendance>>?>()))
            .ReturnsAsync(attendances);

        var handler = new GetAttendanceRecordsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAttendanceRecordsQuery(studentId, null, null), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_RepositoryFailure_ReturnsError()
    {
        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<StudentAttendance, bool>>?>(),
                It.IsAny<Func<IQueryable<StudentAttendance>, IQueryable<StudentAttendance>>?>(),
                It.IsAny<Func<IQueryable<StudentAttendance>, IOrderedQueryable<StudentAttendance>>?>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new GetAttendanceRecordsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetAttendanceRecordsQuery(Guid.NewGuid(), null, null), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetClassEnrollmentsHandlerTests
{
    private readonly Mock<IRepositoryBase<ClassStudentEnrollment, Guid>> _repoMock = new();

    [Fact]
    public async Task Handle_ReturnsEnrollments()
    {
        var studentId = Guid.NewGuid();
        var studentInEnrollment = Student.Create("Ahmed", "Ali", "Fatima", "9876543210", null, "user6", "Pass@123");
        studentInEnrollment.Id = studentId;
        var enrollments = new List<ClassStudentEnrollment>
        {
            new() { Id = Guid.NewGuid(), StudentId = studentId, ClassId = Guid.NewGuid(),
                    Student = studentInEnrollment }
        };

        _repoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ClassStudentEnrollment, bool>>?>(),
                It.IsAny<Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>>?>(),
                It.IsAny<Func<IQueryable<ClassStudentEnrollment>, IOrderedQueryable<ClassStudentEnrollment>>?>()))
            .ReturnsAsync(enrollments);

        var handler = new GetClassEnrollmentsHandler(_repoMock.Object);
        var result = await handler.Handle(new GetClassEnrollmentsQuery(studentId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
    }
}

public class GetMemorizationProgressHandlerTests
{
    private readonly Mock<IRepositoryBase<StudentHadith, Guid>> _hadithRepoMock = new();
    private readonly Mock<IRepositoryBase<StudentQuraanPage, Guid>> _quranRepoMock = new();

    [Fact]
    public async Task Handle_ReturnsMemorizationProgress()
    {
        var studentId = Guid.NewGuid();

        _hadithRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<StudentHadith, bool>>?>(),
                It.IsAny<Func<IQueryable<StudentHadith>, IQueryable<StudentHadith>>?>(),
                It.IsAny<Func<IQueryable<StudentHadith>, IOrderedQueryable<StudentHadith>>?>()))
            .ReturnsAsync(new List<StudentHadith>
            {
                new() { Id = Guid.NewGuid(), StudentId = studentId, HadithId = Guid.NewGuid(), Status = "Memorized" }
            });

        _quranRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<StudentQuraanPage, bool>>?>(),
                It.IsAny<Func<IQueryable<StudentQuraanPage>, IQueryable<StudentQuraanPage>>?>(),
                It.IsAny<Func<IQueryable<StudentQuraanPage>, IOrderedQueryable<StudentQuraanPage>>?>()))
            .ReturnsAsync(new List<StudentQuraanPage>
            {
                new() { Id = Guid.NewGuid(), StudentId = studentId, PageNumber = 1, Status = "Memorized" }
            });

        var handler = new GetMemorizationProgressHandler(_hadithRepoMock.Object, _quranRepoMock.Object);
        var result = await handler.Handle(new GetMemorizationProgressQuery(studentId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.TotalHadithsMemorized);
        Assert.Equal(1, result.Value.TotalQuranPagesMemorized);
    }

    [Fact]
    public async Task Handle_HadithRepositoryFailure_ReturnsError()
    {
        _hadithRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<StudentHadith, bool>>?>(),
                It.IsAny<Func<IQueryable<StudentHadith>, IQueryable<StudentHadith>>?>(),
                It.IsAny<Func<IQueryable<StudentHadith>, IOrderedQueryable<StudentHadith>>?>()))
            .ReturnsAsync(ApplicationErrors.DatabaseError);

        var handler = new GetMemorizationProgressHandler(_hadithRepoMock.Object, _quranRepoMock.Object);
        var result = await handler.Handle(new GetMemorizationProgressQuery(Guid.NewGuid()), CancellationToken.None);

        Assert.True(result.IsError);
    }
}

public class GetPointsHandlerTests
{
    private readonly Mock<IRepositoryBase<Point, Guid>> _pointRepoMock = new();
    private readonly Mock<IRepositoryBase<StudentClassEventsPoint, Guid>> _classEventsRepoMock = new();

    [Fact]
    public async Task Handle_ReturnsStudentPoints()
    {
        var studentId = Guid.NewGuid();
        var points = new List<Point>
        {
            new() { Id = Guid.NewGuid(), StudentId = studentId, PointValue = 10 }
        };

        _pointRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Point, bool>>?>(),
                It.IsAny<Func<IQueryable<Point>, IQueryable<Point>>?>(),
                It.IsAny<Func<IQueryable<Point>, IOrderedQueryable<Point>>?>()))
            .ReturnsAsync(points);

        var handler = new GetPointsHandler(_pointRepoMock.Object, _classEventsRepoMock.Object);
        var result = await handler.Handle(new GetPointsQuery(studentId, null), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Single(result.Value!);
        Assert.Equal(10, result.Value![0].PointValue);
    }

    [Fact]
    public async Task Handle_WithSemesterFilter_FiltersPoints()
    {
        var studentId = Guid.NewGuid();
        var semesterId = Guid.NewGuid();

        _pointRepoMock.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Point, bool>>?>(),
                It.IsAny<Func<IQueryable<Point>, IQueryable<Point>>?>(),
                It.IsAny<Func<IQueryable<Point>, IOrderedQueryable<Point>>?>()))
            .ReturnsAsync(new List<Point>());

        var handler = new GetPointsHandler(_pointRepoMock.Object, _classEventsRepoMock.Object);
        var result = await handler.Handle(new GetPointsQuery(studentId, semesterId), CancellationToken.None);

        Assert.False(result.IsError);
        Assert.Empty(result.Value!);
    }
}
