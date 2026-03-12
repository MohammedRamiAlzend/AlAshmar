using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Courses.UpdateCourse;

public record UpdateCourseCommand(
    Guid Id,
    string EventName
) : ICommand<Result<CourseDto>>;

public class UpdateCourseHandler(IRepositoryBase<Course, Guid> repository)
    : IRequestHandler<UpdateCourseCommand, Result<CourseDto>>
{
    public async Task<Result<CourseDto>> Handle(UpdateCourseCommand command, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetByIdAsync(command.Id);
        if (result.Value == null)
            return new Error("404", "Course not found", ErrorKind.NotFound);

        var course = result.Value;
        course.EventName = command.EventName;

        var updateResult = await repository.UpdateAsync(course);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new CourseDto(course.Id, course.EventName, course.SemesterId, null, []);
    }
}
