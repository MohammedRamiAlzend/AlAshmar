using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Composable, immutable builder for eager-loading <see cref="Course"/> navigation properties.
/// </summary>
public sealed class CourseIncludes : IEntityIncludes<Course>
{
    private readonly IReadOnlyList<Func<IQueryable<Course>, IQueryable<Course>>> _steps;

    private CourseIncludes(IEnumerable<Func<IQueryable<Course>, IQueryable<Course>>> steps)
    {
        _steps = steps.ToList();
    }

    // ── Presets ──────────────────────────────────────────────────────────────

    /// <summary>No navigation properties are loaded.</summary>
    public static readonly CourseIncludes None = new([]);

    /// <summary>Loads only <c>Semester</c>.</summary>
    public static readonly CourseIncludes Basic = None
        .WithSemester();

    /// <summary>Loads all navigation properties.</summary>
    public static readonly CourseIncludes Full = None
        .WithSemester()
        .WithHalaqas();

    // ── Fluent builder methods ────────────────────────────────────────────────

    /// <summary>Includes <c>Course.Semester</c>.</summary>
    public CourseIncludes WithSemester() =>
        Add(q => q.Include(d => d.Semester));

    /// <summary>Includes <c>Course.Halaqas</c>.</summary>
    public CourseIncludes WithHalaqas() =>
        Add(q => q.Include(d => d.Halaqas));

    // ── IEntityIncludes<Course> ──────────────────────────────────────────────

    /// <inheritdoc />
    public Func<IQueryable<Course>, IQueryable<Course>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    // ── Private helpers ───────────────────────────────────────────────────────

    private CourseIncludes Add(Func<IQueryable<Course>, IQueryable<Course>> step) =>
        new(_steps.Append(step));
}
