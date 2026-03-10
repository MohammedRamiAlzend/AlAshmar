using AlAshmar.Application.Common;
using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.UseCases.Students.GetAttendanceRecords;

/// <summary>
/// Query to get student's attendance records.
/// </summary>
public record GetAttendanceRecordsQuery(
    Guid StudentId,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IQuery<Result<List<StudentAttendanceDto>>>;
