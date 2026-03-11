using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Centralizes all eager-loading includes for the <see cref="Teacher"/> entity.
/// Add new navigation property includes here once and they will be applied automatically
/// across all query methods (GetAllAsync, GetPagedAsync, GetAsync, etc.).
/// </summary>
public class TeacherIncludes : IEntityIncludes<Teacher>
{
    /// <summary>
    /// A shared singleton instance to avoid unnecessary allocations.
    /// </summary>
    public static readonly TeacherIncludes Instance = new();

    /// <inheritdoc />
    public Func<IQueryable<Teacher>, IQueryable<Teacher>> Apply() =>
        q => q
            .Include(t => t.RelatedUser)
            .Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo)
            .Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment)
            .Include(t => t.ClassTeacherEnrollments)
            .Include(t => t.GivenPoints).ThenInclude(p => p.Category);
}
