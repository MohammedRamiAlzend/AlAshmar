using AlAshmar.Application.DTOs;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Common;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Application.UseCases.Students.CreateStudent;

public record CreateStudentCommand(
    string Name,
    string FatherName,
    string MotherName,
    string NationalityNumber,
    string? Email,
    string UserName,
    string Password,
    List<CreateStudentContactInfoDto>? ContactInfos
) : IRequest<Result<StudentBasicInfoDto>>;

public class CreateStudentHandler(IRepositoryBase<Student, Guid> repository)
    : IRequestHandler<CreateStudentCommand, Result<StudentBasicInfoDto>>
{
    public async Task<Result<StudentBasicInfoDto>> Handle(CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        var duplicate = await repository.AnyAsync(s => s.NationalityNumber == command.NationalityNumber);
        if (duplicate)
            return ApplicationErrors.NationalityNumberAlreadyExists;

        var student = Student.Create(
            command.Name,
            command.FatherName,
            command.MotherName,
            command.NationalityNumber,
            command.Email,
            command.UserName,
            command.Password
        );

        if (command.ContactInfos is not null)
        {
            foreach (var contact in command.ContactInfos)
            {
                if (string.IsNullOrWhiteSpace(contact.Number))
                    continue;

                student.StudentContactInfos.Add(new StudentContactInfo
                {
                    ContactInfo = new ContactInfo
                    {
                        Number = contact.Number,
                        Email = contact.Email,
                        IsActive = contact.IsActive,
                    }
                });
            }
        }

        var addResult = await repository.AddAsync(student);
        if (addResult.IsError)
            return addResult.Errors;

        return new StudentBasicInfoDto(
            student.Id,
            student.Name
        );
    }
}
