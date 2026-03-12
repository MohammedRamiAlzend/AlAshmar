using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Composable, immutable builder for eager-loading <see cref="Halaqa"/> navigation properties.
/// </summary>
public sealed class HalaqaIncludes : IEntityIncludes<Halaqa>
{
    private readonly IReadOnlyList<Func<IQueryable<Halaqa>, IQueryable<Halaqa>>> _steps;

    private HalaqaIncludes(IEnumerable<Func<IQueryable<Halaqa>, IQueryable<Halaqa>>> steps)
    {
        _steps = steps.ToList();
    }

    // ── Presets ──────────────────────────────────────────────────────────────

    /// <summary>No navigation properties are loaded.</summary>
    public static readonly HalaqaIncludes None = new([]);

    /// <summary>Loads only <c>Course</c>.</summary>
    public static readonly HalaqaIncludes Basic = None
        .WithCourse();

    /// <summary>Loads all navigation properties.</summary>
    public static readonly HalaqaIncludes Full = None
        .WithCourse()
        .WithTeacherEnrollments()
        .WithStudentEnrollments();

    // ── Fluent builder methods ────────────────────────────────────────────────

    /// <summary>Includes <c>Halaqa.Course</c>.</summary>
    public HalaqaIncludes WithCourse() =>
        Add(q => q.Include(h => h.Course));

    /// <summary>Includes <c>Halaqa.ClassTeacherEnrollments → Teacher</c>.</summary>
    public HalaqaIncludes WithTeacherEnrollments() =>
        Add(q => q.Include(h => h.ClassTeacherEnrollments).ThenInclude(cte => cte.Teacher));

    /// <summary>Includes <c>Halaqa.ClassStudentEnrollments → Student</c>.</summary>
    public HalaqaIncludes WithStudentEnrollments() =>
        Add(q => q.Include(h => h.ClassStudentEnrollments).ThenInclude(cse => cse.Student));

    // ── IEntityIncludes<Halaqa> ──────────────────────────────────────────────

    /// <inheritdoc />
    public Func<IQueryable<Halaqa>, IQueryable<Halaqa>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    // ── Private helpers ───────────────────────────────────────────────────────

    private HalaqaIncludes Add(Func<IQueryable<Halaqa>, IQueryable<Halaqa>> step) =>
        new(_steps.Append(step));
}
