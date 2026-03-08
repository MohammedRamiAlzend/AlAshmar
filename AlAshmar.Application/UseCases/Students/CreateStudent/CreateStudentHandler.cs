using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.CreateStudent;

public record CreateStudentCommand(
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    string UserName,
    string Password
) : ICommand<Result<StudentBasicInfoDto>>;

public class CreateStudentHandler(IRepositoryBase<Student, Guid> repository)
    : ICommandHandler<CreateStudentCommand, Result<StudentBasicInfoDto>>
{
    public async Task<Result<StudentBasicInfoDto>> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        var student = Student.Create(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email,
            command.UserName,
            command.Password
        );

        var addResult = await repository.AddAsync(student);
        if (addResult.IsError)
            return addResult.Errors;

        return new StudentBasicInfoDto(
            student.Id,
            student.Name
        );
    }
}
