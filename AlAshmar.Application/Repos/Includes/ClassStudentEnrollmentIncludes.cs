using AlAshmar.Domain.Entities.Students;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class ClassStudentEnrollmentIncludes : IEntityIncludes<ClassStudentEnrollment>
{
    private readonly IReadOnlyList<Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>>> _steps;

    private ClassStudentEnrollmentIncludes(IEnumerable<Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly ClassStudentEnrollmentIncludes None = new([]);

    public static readonly ClassStudentEnrollmentIncludes Basic = None
        .WithStudent();

    public static readonly ClassStudentEnrollmentIncludes Full = None
        .WithStudent()
        .WithHalaqa();

    public static readonly ClassStudentEnrollmentIncludes Instance = Full;

    public ClassStudentEnrollmentIncludes WithStudent() =>
        Add(q => q.Include(e => e.Student));

    public ClassStudentEnrollmentIncludes WithHalaqa() =>
        Add(q => q.Include(e => e.Halaqa));

    public Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private ClassStudentEnrollmentIncludes Add(Func<IQueryable<ClassStudentEnrollment>, IQueryable<ClassStudentEnrollment>> step) =>
        new(_steps.Append(step));
}
