using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Courses.UpdateCourse;

public record UpdateCourseCommand(
    Guid Id,
    string EventName
) : IRequest<Result<CourseDto>>;

public class UpdateCourseHandler(IRepositoryBase<Course, Guid> repository)
    : IRequestHandler<UpdateCourseCommand, Result<CourseDto>>
{
    public async Task<Result<CourseDto>> Handle(UpdateCourseCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return ApplicationErrors.CourseNotFound;

        var course = result.Value;
        course.CourseName = command.EventName;

        var updateResult = await repository.UpdateAsync(course);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new CourseDto(course.Id, course.CourseName, course.SemesterId, null, []);
    }
}
