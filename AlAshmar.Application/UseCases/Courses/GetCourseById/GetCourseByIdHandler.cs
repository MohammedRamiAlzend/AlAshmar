using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Academic;

namespace AlAshmar.Application.UseCases.Courses.GetCourseById;

public record GetCourseByIdQuery(Guid Id) : IQuery<Result<CourseDto?>>;

public class GetCourseByIdHandler(IRepositoryBase<Course, Guid> repository)
    : IRequestHandler<GetCourseByIdQuery, Result<CourseDto?>>
{
    public async Task<Result<CourseDto?>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetAllAsync(
            d => d.Id == query.Id,
            CourseIncludes.Full.Apply());

        if (result.IsError)
            return result.Errors;

        var course = result.Value.FirstOrDefault();
        if (course == null)
            return ApplicationErrors.CourseNotFound;

        var semesterDto = course.Semester is not null
            ? new SemesterDto(course.Semester.Id, course.Semester.StartDate, course.Semester.EndDate, course.Semester.Name)
            : null;

        var halaqaDtos = course.Halaqas
            .Select(h => new HalaqaDto(h.Id, h.HalaqaName, h.CourseId, null))
            .ToList();

        return new CourseDto(course.Id, course.CourseName, course.SemesterId, semesterDto, halaqaDtos);
    }
}
