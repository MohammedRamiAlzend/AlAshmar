using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;
using MediatR;

namespace AlAshmar.Application.UseCases.Courses.GetCoursesBySemester;

public record GetCoursesBySemesterQuery(Guid SemesterId) : IQuery<Result<List<CourseDto>>>;

public class GetCoursesBySemesterHandler(IRepositoryBase<Course, Guid> repository)
    : IRequestHandler<GetCoursesBySemesterQuery, Result<List<CourseDto>>>
{
    public async Task<Result<List<CourseDto>>> Handle(GetCoursesBySemesterQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            d => d.SemesterId == query.SemesterId,
            CourseIncludes.Basic.Apply());

        if (result.IsError)
            return result.Errors;

        var courses = result.Value
            .Select(d => new CourseDto(d.Id, d.EventName, d.SemesterId, null, []))
            .ToList();

        return courses;
    }
}
