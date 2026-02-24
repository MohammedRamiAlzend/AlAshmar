using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Students.CreateStudent;

public record CreateStudentCommand(
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    Guid? UserId
) : ICommand<Result<StudentDto>>;

public class CreateStudentHandler : ICommandHandler<CreateStudentCommand, Result<StudentDto>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public CreateStudentHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<StudentDto>> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        var student = Student.Create(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email,
            command.UserId
        );

        var addResult = await _repository.AddAsync(student);
        if (addResult.IsError)
            return addResult.Errors;

        return new StudentDto(
            student.Id,
            student.Name,
            student.FatherName,
            student.MotherName,
            student.NationalityNumber,
            student.Email,
            student.UserId
        );
    }
}
