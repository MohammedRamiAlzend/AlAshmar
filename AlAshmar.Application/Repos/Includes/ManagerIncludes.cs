using AlAshmar.Domain.Entities.Managers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Composable, immutable builder for eager-loading <see cref="Manager"/> navigation properties.
/// <para>
/// Use the named presets (<see cref="None"/>, <see cref="Basic"/>, <see cref="Full"/>) as a
/// starting point and chain <c>With*()</c> methods to add exactly the navigation properties
/// you need. Every <c>With*()</c> call returns a <b>new</b> <see cref="ManagerIncludes"/>
/// instance — the original is never mutated — so presets are safe to reuse across
/// different call sites.
/// </para>
/// <example><code>
/// // Pick a preset:
/// var transform = ManagerIncludes.Basic.Apply();           // User only
/// var transform = ManagerIncludes.Full.Apply();            // everything
///
/// // Or compose a custom profile on the fly:
/// var transform = ManagerIncludes.None
///     .WithUser()
///     .WithAttachments()
///     .Apply();
/// </code></example>
/// </summary>
public sealed class ManagerIncludes : IEntityIncludes<Manager>
{
    private readonly IReadOnlyList<Func<IQueryable<Manager>, IQueryable<Manager>>> _steps;

    private ManagerIncludes(IEnumerable<Func<IQueryable<Manager>, IQueryable<Manager>>> steps)
    {
        _steps = steps.ToList();
    }

    // ── Presets ──────────────────────────────────────────────────────────────

    /// <summary>No navigation properties are loaded.</summary>
    public static readonly ManagerIncludes None = new([]);

    /// <summary>Loads only <c>User</c>. Suited for lightweight list views.</summary>
    public static readonly ManagerIncludes Basic = None
        .WithUser();

    /// <summary>
    /// Loads all navigation properties. Suited for full detail views.
    /// <para>
    /// This is the default and is equivalent to the pre-existing
    /// <see cref="Instance"/> for backward compatibility.
    /// </para>
    /// </summary>
    public static readonly ManagerIncludes Full = None
        .WithUser()
        .WithContactInfos()
        .WithAttachments();

    /// <summary>
    /// Default (full) includes — retained for backward compatibility.
    /// Equivalent to <see cref="Full"/>.
    /// </summary>
    public static readonly ManagerIncludes Instance = Full;

    // ── Fluent builder methods ────────────────────────────────────────────────

    /// <summary>Includes <c>Manager.User</c>.</summary>
    public ManagerIncludes WithUser() =>
        Add(q => q.Include(m => m.User));

    /// <summary>Includes <c>ManagerContactInfos → ContactInfo</c>.</summary>
    public ManagerIncludes WithContactInfos() =>
        Add(q => q.Include(m => m.ManagerContactInfos).ThenInclude(mc => mc.ContactInfo));

    /// <summary>Includes <c>ManagerAttachments → Attachment</c>.</summary>
    public ManagerIncludes WithAttachments() =>
        Add(q => q.Include(m => m.ManagerAttachments).ThenInclude(ma => ma.Attachment));

    // ── IEntityIncludes<Manager> ──────────────────────────────────────────────

    /// <inheritdoc />
    public Func<IQueryable<Manager>, IQueryable<Manager>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    // ── Private helpers ───────────────────────────────────────────────────────

    private ManagerIncludes Add(Func<IQueryable<Manager>, IQueryable<Manager>> step) =>
        new(_steps.Append(step));
}
