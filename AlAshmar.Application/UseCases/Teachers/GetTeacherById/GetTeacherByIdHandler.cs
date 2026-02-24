using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs;
using AlAshmar.Domain.Commons;
using AlAshmar.Domain.Entities.Teachers;
using AlAshmar.Application.Repos;

namespace AlAshmar.Application.UseCases.Teachers.GetTeacherById;

public record GetTeacherByIdQuery(Guid Id) : IQuery<Result<TeacherDto?>>;

public class GetTeacherByIdHandler : IQueryHandler<GetTeacherByIdQuery, Result<TeacherDto?>>
{
    private readonly IRepositoryBase<Teacher, Guid> _repository;

    public GetTeacherByIdHandler(IRepositoryBase<Teacher, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<Result<TeacherDto?>> Handle(GetTeacherByIdQuery query, CancellationToken cancellationToken = default)
    {
        var teacher = await _repository.GetByIdAsync(query.Id);
        
        if (teacher.Value == null)
            return new Error("404", "Teacher not found", ErrorKind.NotFound);

        return new TeacherDto(
            teacher.Value.Id,
            teacher.Value.Name,
            teacher.Value.FatherName,
            teacher.Value.MotherName,
            teacher.Value.NationalityNumber,
            teacher.Value.Email,
            teacher.Value.UserId
        );
    }
}
