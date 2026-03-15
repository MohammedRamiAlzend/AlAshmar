using AlAshmar.Application.DTOs;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Application.UseCases.Teachers.CreateTeacher;

public record CreateTeacherCommand(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email,
    string UserName,
    string Password
) : IRequest<Result<TeacherDto>>;

public class CreateTeacherHandler(IRepositoryBase<Teacher, Guid> repository) :
    IRequestHandler<CreateTeacherCommand, Result<TeacherDto>>
{
    public async Task<Result<TeacherDto>> Handle(CreateTeacherCommand command, CancellationToken cancellationToken = default)
    {
        var duplicate = await repository.AnyAsync(t => t.NationalityNumber == command.NationalityNumber);
        if (duplicate)
            return ApplicationErrors.NationalityNumberAlreadyExists;

        var teacher = Teacher.Create(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email,
            command.UserName,
            command.Password
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
