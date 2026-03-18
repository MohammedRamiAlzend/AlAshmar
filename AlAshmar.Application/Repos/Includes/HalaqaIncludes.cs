using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class HalaqaIncludes : IEntityIncludes<Halaqa>
{
    private readonly IReadOnlyList<Func<IQueryable<Halaqa>, IQueryable<Halaqa>>> _steps;

    private HalaqaIncludes(IEnumerable<Func<IQueryable<Halaqa>, IQueryable<Halaqa>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly HalaqaIncludes None = new([]);

    public static readonly HalaqaIncludes Basic = None
        .WithCourse();

    public static readonly HalaqaIncludes Full = None
        .WithCourse()
        .WithTeacherEnrollments()
        .WithStudentEnrollments();

    public static readonly HalaqaIncludes Instance = Full;

    public HalaqaIncludes WithCourse() =>
        Add(q => q.Include(h => h.Course));

    public HalaqaIncludes WithTeacherEnrollments() =>
        Add(q => q.Include(h => h.ClassTeacherEnrollments).ThenInclude(cte => cte.Teacher));

    public HalaqaIncludes WithStudentEnrollments() =>
        Add(q => q.Include(h => h.ClassStudentEnrollments).ThenInclude(cse => cse.Student));

    public Func<IQueryable<Halaqa>, IQueryable<Halaqa>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private HalaqaIncludes Add(Func<IQueryable<Halaqa>, IQueryable<Halaqa>> step) =>
        new(_steps.Append(step));
}
