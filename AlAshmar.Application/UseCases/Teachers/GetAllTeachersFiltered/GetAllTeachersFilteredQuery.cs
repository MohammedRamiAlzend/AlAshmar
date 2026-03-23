using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Teachers.GetAllTeachersFiltered;

public record GetAllTeachersFilteredQuery(
    int? PageNumber = null,
    int? PageSize = null,
    Guid? ClassId = null,
    Guid? SemesterId = null,
    Guid? EventId = null
) : IQuery<Result<List<TeacherDto>>>;
