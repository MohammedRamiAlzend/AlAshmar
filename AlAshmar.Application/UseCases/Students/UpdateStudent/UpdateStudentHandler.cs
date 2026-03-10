using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Students;
using AlAshmar.Application.Repos;
using AlAshmar.Application.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.UpdateStudent;

public record UpdateStudentCommand(
    Guid Id,
    string Name,
    string FatherName,
    string MotherName,
    string? NationalityNumber,
    string? Email
) : ICommand<Result<Success>>;

public class UpdateStudentHandler : ICommandHandler<UpdateStudentCommand, Result<Success>>
{
    private readonly IRepositoryBase<Student, Guid> _repository;

    public UpdateStudentHandler(IRepositoryBase<Student, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<Success>> Handle(UpdateStudentCommand command, CancellationToken cancellationToken = default)
    {
        var studentResult = await _repository.GetByIdAsync(command.Id);
        if (studentResult.Value == null)
            return new Error("404", "Student not found", ErrorKind.NotFound);

        var student = studentResult.Value;
        student.UpdateBasicInfo(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email
        );

        var updateResult = await _repository.UpdateAsync(student);
        if (updateResult.IsError)
            return updateResult.Errors;

        return new Success("Student updated successfully");
    }
}
