using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Courses.GetAllCourses;

public record GetAllCoursesQuery : IQuery<Result<List<CourseDto>>>;

public class GetAllCoursesHandler(IRepositoryBase<Course, Guid> repository)
    : IRequestHandler<GetAllCoursesQuery, Result<List<CourseDto>>>
{
    public async Task<Result<List<CourseDto>>> Handle(GetAllCoursesQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(transform: CourseIncludes.Basic.Apply());
        if (result.IsError)
            return result.Errors;

        var courses = result.Value
            .Select(d => new CourseDto(d.Id, d.EventName, d.SemesterId, null, []))
            .ToList();

        return courses;
    }
}
