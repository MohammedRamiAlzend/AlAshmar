using AlAshmar.Domain.DTOs.Domain;

namespace AlAshmar.Application.UseCases.Students.GetAttendanceRecords;

public record GetAttendanceRecordsQuery(
    Guid StudentId,
    DateTime? FromDate = null,
    DateTime? ToDate = null
) : IQuery<Result<List<StudentAttendanceDto>>>;
