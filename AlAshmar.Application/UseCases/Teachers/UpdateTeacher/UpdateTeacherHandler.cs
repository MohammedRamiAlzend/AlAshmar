using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Teachers.UpdateTeacher;

public record UpdateTeacherCommand(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email
) : ICommand<Result<TeacherDto>>;

public class UpdateTeacherHandler : ICommandHandler<UpdateTeacherCommand, Result<TeacherDto>>
{
    private readonly IRepositoryBase<Teacher, Guid> _repository;

    public UpdateTeacherHandler(IRepositoryBase<Teacher, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<TeacherDto>> Handle(UpdateTeacherCommand command, CancellationToken cancellationToken = default)
    {
        var teacherResult = await _repository.GetByIdAsync(command.Id);
        if (teacherResult.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        var teacher = teacherResult.Value;
        teacher.UpdateBasicInfo(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email
        );

        var updateResult = await _repository.UpdateAsync(teacher);
        if (updateResult.IsError)
            return updateResult.Errors;

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
