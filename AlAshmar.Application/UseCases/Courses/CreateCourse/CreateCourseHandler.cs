using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Courses.CreateCourse;

public record CreateCourseCommand(
    string EventName,
    Guid SemesterId
) : IRequest<Result<CourseDto>>;

public class CreateCourseHandler(
    IRepositoryBase<Course, Guid> courseRepository,
    IRepositoryBase<Semester, Guid> semesterRepository)
    : IRequestHandler<CreateCourseCommand, Result<CourseDto>>
{
    public async Task<Result<CourseDto>> Handle(CreateCourseCommand command, CancellationToken cancellationToken = default)
    {
        var semesterExists = await semesterRepository.AnyAsync(s => s.Id == command.SemesterId);
        if (!semesterExists)
            return ApplicationErrors.SemesterNotFound;

        var course = new Course
        {
            CourseName = command.EventName,
            SemesterId = command.SemesterId
        };

        var addResult = await courseRepository.AddAsync(course);
        if (addResult.IsError)
            return addResult.Errors;

        return new CourseDto(course.Id, course.CourseName, course.SemesterId, null, []);
    }
}
