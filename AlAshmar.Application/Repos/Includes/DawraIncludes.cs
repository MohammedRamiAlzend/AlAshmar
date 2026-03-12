using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Composable, immutable builder for eager-loading <see cref="Dawra"/> navigation properties.
/// </summary>
public sealed class DawraIncludes : IEntityIncludes<Dawra>
{
    private readonly IReadOnlyList<Func<IQueryable<Dawra>, IQueryable<Dawra>>> _steps;

    private DawraIncludes(IEnumerable<Func<IQueryable<Dawra>, IQueryable<Dawra>>> steps)
    {
        _steps = steps.ToList();
    }

    // ── Presets ──────────────────────────────────────────────────────────────

    /// <summary>No navigation properties are loaded.</summary>
    public static readonly DawraIncludes None = new([]);

    /// <summary>Loads only <c>Semester</c>.</summary>
    public static readonly DawraIncludes Basic = None
        .WithSemester();

    /// <summary>Loads all navigation properties.</summary>
    public static readonly DawraIncludes Full = None
        .WithSemester()
        .WithHalaqas();

    // ── Fluent builder methods ────────────────────────────────────────────────

    /// <summary>Includes <c>Dawra.Semester</c>.</summary>
    public DawraIncludes WithSemester() =>
        Add(q => q.Include(d => d.Semester));

    /// <summary>Includes <c>Dawra.Halaqas</c>.</summary>
    public DawraIncludes WithHalaqas() =>
        Add(q => q.Include(d => d.Halaqas));

    // ── IEntityIncludes<Dawra> ──────────────────────────────────────────────

    /// <inheritdoc />
    public Func<IQueryable<Dawra>, IQueryable<Dawra>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    // ── Private helpers ───────────────────────────────────────────────────────

    private DawraIncludes Add(Func<IQueryable<Dawra>, IQueryable<Dawra>> step) =>
        new(_steps.Append(step));
}
