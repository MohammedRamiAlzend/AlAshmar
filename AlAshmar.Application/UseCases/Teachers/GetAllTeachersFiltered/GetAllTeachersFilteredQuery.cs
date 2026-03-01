using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Teachers.GetAllTeachersFiltered;

/// <summary>
/// Query for getting filtered teachers with support for OR operations on filter parameters.
/// All filter parameters are nullable to support flexible filtering.
/// </summary>
/// <param name="PageNumber">Page number for pagination (1-based)</param>
/// <param name="PageSize">Number of items per page</param>
/// <param name="ClassId">Filter by class ID (nullable - supports OR operation)</param>
/// <param name="SemesterId">Filter by semester ID (nullable - supports OR operation)</param>
/// <param name="EventId">Filter by event ID (nullable - supports OR operation)</param>
public record GetAllTeachersFilteredQuery(
    int? PageNumber = null,
    int? PageSize = null,
    Guid? ClassId = null,
    Guid? SemesterId = null,
    Guid? EventId = null
) : IQuery<Result<List<TeacherDto>>>;
