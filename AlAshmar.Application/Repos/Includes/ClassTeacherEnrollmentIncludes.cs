using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class ClassTeacherEnrollmentIncludes : IEntityIncludes<ClassTeacherEnrollment>
{
    private readonly IReadOnlyList<Func<IQueryable<ClassTeacherEnrollment>, IQueryable<ClassTeacherEnrollment>>> _steps;

    private ClassTeacherEnrollmentIncludes(IEnumerable<Func<IQueryable<ClassTeacherEnrollment>, IQueryable<ClassTeacherEnrollment>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly ClassTeacherEnrollmentIncludes None = new([]);

    public static readonly ClassTeacherEnrollmentIncludes Basic = None
        .WithTeacher();

    public static readonly ClassTeacherEnrollmentIncludes Full = None
        .WithTeacher()
        .WithHalaqa();

    public static readonly ClassTeacherEnrollmentIncludes Instance = Full;

    public ClassTeacherEnrollmentIncludes WithTeacher() =>
        Add(q => q.Include(e => e.Teacher));

    public ClassTeacherEnrollmentIncludes WithHalaqa() =>
        Add(q => q.Include(e => e.Halaqa));

    public Func<IQueryable<ClassTeacherEnrollment>, IQueryable<ClassTeacherEnrollment>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private ClassTeacherEnrollmentIncludes Add(Func<IQueryable<ClassTeacherEnrollment>, IQueryable<ClassTeacherEnrollment>> step) =>
        new(_steps.Append(step));
}
