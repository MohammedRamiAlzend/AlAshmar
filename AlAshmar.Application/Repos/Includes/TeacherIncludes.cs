using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Composable, immutable builder for eager-loading <see cref="Teacher"/> navigation properties.
/// <para>
/// Use the named presets (<see cref="None"/>, <see cref="Basic"/>, <see cref="Full"/>) as a
/// starting point and chain <c>With*()</c> methods to add exactly the navigation properties
/// you need. Every <c>With*()</c> call returns a <b>new</b> <see cref="TeacherIncludes"/>
/// instance — the original is never mutated — so presets are safe to reuse across
/// different call sites.
/// </para>
/// <example><code>
/// // Pick a preset:
/// var transform = TeacherIncludes.Basic.Apply();           // RelatedUser only
/// var transform = TeacherIncludes.Full.Apply();            // everything
///
/// // Or compose a custom profile on the fly:
/// var transform = TeacherIncludes.None
///     .WithRelatedUser()
///     .WithGivenPoints()
///     .Apply();
/// </code></example>
/// </summary>
public sealed class TeacherIncludes : IEntityIncludes<Teacher>
{
    private readonly IReadOnlyList<Func<IQueryable<Teacher>, IQueryable<Teacher>>> _steps;

    private TeacherIncludes(IEnumerable<Func<IQueryable<Teacher>, IQueryable<Teacher>>> steps)
    {
        _steps = steps.ToList();
    }

    // ── Presets ──────────────────────────────────────────────────────────────

    /// <summary>No navigation properties are loaded.</summary>
    public static readonly TeacherIncludes None = new([]);

    /// <summary>Loads only <c>RelatedUser</c>. Suited for lightweight list views.</summary>
    public static readonly TeacherIncludes Basic = None
        .WithRelatedUser();

    /// <summary>
    /// Loads all navigation properties. Suited for full detail views.
    /// <para>
    /// This is the default and is equivalent to the pre-existing
    /// <see cref="Instance"/> for backward compatibility.
    /// </para>
    /// </summary>
    public static readonly TeacherIncludes Full = None
        .WithRelatedUser()
        .WithContactInfos()
        .WithAttachments()
        .WithEnrollments()
        .WithGivenPoints();

    /// <summary>
    /// Default (full) includes — retained for backward compatibility.
    /// Equivalent to <see cref="Full"/>.
    /// </summary>
    public static readonly TeacherIncludes Instance = Full;

    // ── Fluent builder methods ────────────────────────────────────────────────

    /// <summary>Includes <c>Teacher.RelatedUser</c>.</summary>
    public TeacherIncludes WithRelatedUser() =>
        Add(q => q.Include(t => t.RelatedUser));

    /// <summary>Includes <c>TeacherContactInfos → ContactInfo</c>.</summary>
    public TeacherIncludes WithContactInfos() =>
        Add(q => q.Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo));

    /// <summary>Includes <c>TeacherAttachments → Attachment</c>.</summary>
    public TeacherIncludes WithAttachments() =>
        Add(q => q.Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment));

    /// <summary>Includes <c>ClassTeacherEnrollments</c>.</summary>
    public TeacherIncludes WithEnrollments() =>
        Add(q => q.Include(t => t.ClassTeacherEnrollments));

    /// <summary>Includes <c>GivenPoints → Category</c>.</summary>
    public TeacherIncludes WithGivenPoints() =>
        Add(q => q.Include(t => t.GivenPoints).ThenInclude(p => p.Category));

    // ── IEntityIncludes<Teacher> ──────────────────────────────────────────────

    /// <inheritdoc />
    public Func<IQueryable<Teacher>, IQueryable<Teacher>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    // ── Private helpers ───────────────────────────────────────────────────────

    private TeacherIncludes Add(Func<IQueryable<Teacher>, IQueryable<Teacher>> step) =>
        new(_steps.Append(step));
}
