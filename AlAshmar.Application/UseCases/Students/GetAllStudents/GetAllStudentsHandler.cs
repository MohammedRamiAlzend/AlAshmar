 
using AlAshmar.Application.Repos;
using AlAshmar.Application.Repos.Includes;
using AlAshmar.Domain.DTOs.Domain;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Application.UseCases.Students.GetAllStudents;

public record GetAllStudentsQuery : IQuery<Result<List<StudentListItemDto>>>;

public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, Result<List<StudentListItemDto>>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public GetAllStudentsHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<StudentListItemDto>>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken = default)
    {
        var studentsResult = await _repository.GetAllAsync(
            transform: StudentIncludes.Basic.Apply()
        );

        if (studentsResult.IsError)
            return studentsResult.Errors;

        var students = studentsResult.Value
            .Select(s => new StudentListItemDto(
                s.Id,
                s.Name,
                s.FatherName,
                s.MotherName,
                s.NationalityNumber,
                s.Email,
                s.User?.UserName,
                s.User?.RoleId != null ? GetRoleType(s.User.RoleId) : null
            ))
            .ToList();

        return students;
    }

    private static string GetRoleType(Guid roleId)
    {

        return roleId == Guid.Parse("00000000-0000-0000-0000-000000000001") ? "Student" : "Unknown";
    }
}
