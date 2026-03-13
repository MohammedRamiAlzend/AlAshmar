using AlAshmar.Domain.Entities.Students;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Composable, immutable builder for eager-loading <see cref="Student"/> navigation properties.
/// <para>
/// Use the named presets (<see cref="None"/>, <see cref="Basic"/>, <see cref="Full"/>) as a
/// starting point and chain <c>With*()</c> methods to add exactly the navigation properties
/// you need. Every <c>With*()</c> call returns a <b>new</b> <see cref="StudentIncludes"/>
/// instance — the original is never mutated — so presets are safe to reuse across
/// different call sites.
/// </para>
/// <example><code>
/// // Pick a preset:
/// var transform = StudentIncludes.Basic.Apply();          // User only
/// var transform = StudentIncludes.Full.Apply();           // everything
///
/// // Or compose a custom profile on the fly:
/// var transform = StudentIncludes.None
///     .WithUser()
///     .WithPoints()
///     .Apply();
/// </code></example>
/// </summary>
public sealed class StudentIncludes : IEntityIncludes<Student>
{
    private readonly IReadOnlyList<Func<IQueryable<Student>, IQueryable<Student>>> _steps;

    private StudentIncludes(IEnumerable<Func<IQueryable<Student>, IQueryable<Student>>> steps)
    {
        _steps = steps.ToList();
    }

    // ── Presets ──────────────────────────────────────────────────────────────

    /// <summary>No navigation properties are loaded.</summary>
    public static readonly StudentIncludes None = new([]);

    /// <summary>Loads only <c>User</c>. Suited for lightweight list views.</summary>
    public static readonly StudentIncludes Basic = None
        .WithUser();

    /// <summary>
    /// Loads all navigation properties. Suited for full detail views.
    /// <para>
    /// This is the default and is equivalent to the pre-existing
    /// <see cref="Instance"/> for backward compatibility.
    /// </para>
    /// </summary>
    public static readonly StudentIncludes Full = None
        .WithUser()
        .WithContactInfos()
        .WithAttachments()
        .WithHadiths()
        .WithQuraanPages()
        .WithClassEventsPoints()
        .WithPoints();

    /// <summary>
    /// Default (full) includes — retained for backward compatibility.
    /// Equivalent to <see cref="Full"/>.
    /// </summary>
    public static readonly StudentIncludes Instance = Full;

    // ── Fluent builder methods ────────────────────────────────────────────────

    /// <summary>Includes <c>Student.User</c>.</summary>
    public StudentIncludes WithUser() =>
        Add(q => q.Include(s => s.User));

    /// <summary>Includes <c>StudentContactInfos → ContactInfo</c>.</summary>
    public StudentIncludes WithContactInfos() =>
        Add(q => q.Include(s => s.StudentContactInfos).ThenInclude(sc => sc.ContactInfo));

    /// <summary>Includes <c>StudentAttachments → Attachment → Extention</c>.</summary>
    public StudentIncludes WithAttachments() =>
        Add(q => q.Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment).ThenInclude(a => a.Extention));

    /// <summary>Includes <c>StudentHadiths → Hadith → Book</c>.</summary>
    public StudentIncludes WithHadiths() =>
        Add(q => q.Include(s => s.StudentHadiths).ThenInclude(h => h.Hadith).ThenInclude(h => h.Book));

    /// <summary>Includes <c>StudentQuraanPages</c>.</summary>
    public StudentIncludes WithQuraanPages() =>
        Add(q => q.Include(s => s.StudentQuraanPages));

    /// <summary>Includes <c>StudentClassEventsPoints</c>.</summary>
    public StudentIncludes WithClassEventsPoints() =>
        Add(q => q.Include(s => s.StudentClassEventsPoints));

    /// <summary>Includes <c>Points → Category</c>.</summary>
    public StudentIncludes WithPoints() =>
        Add(q => q.Include(s => s.Points).ThenInclude(p => p.Category));

    // ── IEntityIncludes<Student> ──────────────────────────────────────────────

    /// <inheritdoc />
    public Func<IQueryable<Student>, IQueryable<Student>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    // ── Private helpers ───────────────────────────────────────────────────────

    private StudentIncludes Add(Func<IQueryable<Student>, IQueryable<Student>> step) =>
        new(_steps.Append(step));
}
