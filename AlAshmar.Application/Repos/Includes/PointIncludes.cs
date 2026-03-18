using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class PointIncludes : IEntityIncludes<Point>
{
    private readonly IReadOnlyList<Func<IQueryable<Point>, IQueryable<Point>>> _steps;

    private PointIncludes(IEnumerable<Func<IQueryable<Point>, IQueryable<Point>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly PointIncludes None = new([]);

    public static readonly PointIncludes Basic = None
        .WithCategory();

    public static readonly PointIncludes Full = None
        .WithCategory()
        .WithStudent()
        .WithHalaqa()
        .WithCourse()
        .WithGivenByTeacher();

    public static readonly PointIncludes Instance = Basic;

    public PointIncludes WithCategory() =>
        Add(q => q.Include(p => p.Category));

    public PointIncludes WithStudent() =>
        Add(q => q.Include(p => p.Student));

    public PointIncludes WithHalaqa() =>
        Add(q => q.Include(p => p.Halaqa));

    public PointIncludes WithCourse() =>
        Add(q => q.Include(p => p.Course));

    public PointIncludes WithGivenByTeacher() =>
        Add(q => q.Include(p => p.GivenByTeacher));

    public Func<IQueryable<Point>, IQueryable<Point>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private PointIncludes Add(Func<IQueryable<Point>, IQueryable<Point>> step) =>
        new(_steps.Append(step));
}
