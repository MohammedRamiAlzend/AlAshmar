using AlAshmar.Application.DTOs.Domain;
using AlAshmar.Application.Repos;
using AlAshmar.Domain.Entities.Students;

namespace AlAshmar.Application.UseCases.Students.GetAttendanceRecords;

public class GetAttendanceRecordsHandler(IRepositoryBase<StudentAttendance, Guid> repository)
    : IRequestHandler<GetAttendanceRecordsQuery, Result<List<StudentAttendanceDto>>>
{
    public async Task<Result<List<StudentAttendanceDto>>> Handle(GetAttendanceRecordsQuery query, CancellationToken cancellationToken = default)
    {
        var attendances = await repository.GetAllAsync(
            a => a.ClassStudentId == query.StudentId &&
                 (!query.FromDate.HasValue || a.StartDate >= query.FromDate.Value) &&
                 (!query.ToDate.HasValue || a.EndDate <= query.ToDate.Value));

        if (attendances.IsError) return attendances.Errors;

        var attendanceDtos = attendances.Value
            .Select(a => new StudentAttendanceDto(
                a.Id, a.StartDate, a.EndDate, a.ClassStudentId)).ToList();

        return attendanceDtos;
    }
}
