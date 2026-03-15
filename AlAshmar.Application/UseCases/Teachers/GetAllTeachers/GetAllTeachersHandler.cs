using AlAshmar.Application.DTOs;
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Application.UseCases.Teachers.GetAllTeachers;

public record GetAllTeachersQuery : IQuery<Result<List<TeacherDto>>>;

public class GetAllTeachersHandler : IRequestHandler<GetAllTeachersQuery, Result<List<TeacherDto>>>
{
    private readonly IRepositoryBase<Teacher, Guid> _repository;

    public GetAllTeachersHandler(IRepositoryBase<Teacher, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<TeacherDto>>> Handle(GetAllTeachersQuery query, CancellationToken cancellationToken = default)
    {
        var teachersResult = await _repository.GetAllAsync(transform: TeacherIncludes.None.Apply());
        if (teachersResult.IsError)
            return teachersResult.Errors;

        var teachers = teachersResult.Value
            .Select(t => new TeacherDto(
                t.Id,
                t.Name,
                t.FatherName,
                t.MotherName,
                t.NationalityNumber,
                t.Email,
                t.UserId
            ))
            .ToList();

        return teachers;
    }
}
