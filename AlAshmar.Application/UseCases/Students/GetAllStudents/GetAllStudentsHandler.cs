using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.GetAllStudents;

public record GetAllStudentsQuery : IQuery<Result<List<StudentDto>>>;

public class GetAllStudentsHandler : IQueryHandler<GetAllStudentsQuery, Result<List<StudentDto>>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public GetAllStudentsHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<List<StudentDto>>> Handle(GetAllStudentsQuery query, CancellationToken cancellationToken = default)
    {
        var studentsResult = await _repository.GetAllAsync();
        if (studentsResult.IsError)
            return studentsResult.Errors;

        var students = studentsResult.Value
            .Select(s => new StudentDto(
                s.Id,
                s.Name,
                s.FatherName,
                s.MotherName,
                s.NationalityNumber,
                s.Email,
                s.UserId
            ))
            .ToList();

        return students;
    }
}
