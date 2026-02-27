using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Teachers.CreateTeacher;

public record CreateTeacherCommand(
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email,
    string UserName,
    string Password
) : ICommand<Result<TeacherDto>>;

public class CreateTeacherHandler(IRepositoryBase<Teacher, Guid> repository) : 
    ICommandHandler<CreateTeacherCommand, Result<TeacherDto>>
{
    public async Task<Result<TeacherDto>> Handle(CreateTeacherCommand command, CancellationToken cancellationToken = default)
    {
        var teacher = Teacher.Create(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email,
            command.UserId
        );

        var addResult = await repository.AddAsync(teacher);
        if (addResult.IsError)
            return addResult.Errors;

        return new TeacherDto(
            teacher.Id,
            teacher.Name,
            teacher.FatherName,
            teacher.MotherName,
            teacher.NationalityNumber,
            teacher.Email,
            teacher.UserId
        );
    }
}
