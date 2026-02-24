using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.GetStudentById;

public record GetStudentByIdQuery(Guid Id) : IQuery<Result<StudentDto?>>;

public class GetStudentByIdHandler : IQueryHandler<GetStudentByIdQuery, Result<StudentDto?>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public GetStudentByIdHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<StudentDto?>> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var student = await _repository.GetByIdAsync(query.Id);
        
        if (student.Value == null)
            return new Error("404", "Student not found", ErrorKind.NotFound);

        return new StudentDto(
            student.Value.Id,
            student.Value.Name,
            student.Value.FatherName,
            student.Value.MotherName,
            student.Value.NationalityNumber,
            student.Value.Email,
            student.Value.UserId
        );
    }
}
