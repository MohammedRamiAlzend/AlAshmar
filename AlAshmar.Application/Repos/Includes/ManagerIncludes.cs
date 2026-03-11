using AlAshmar.Domain.Entities.Managers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Centralizes all eager-loading includes for the <see cref="Manager"/> entity.
/// Add new navigation property includes here once and they will be applied automatically
/// across all query methods (GetAllAsync, GetPagedAsync, GetAsync, etc.).
/// </summary>
public class ManagerIncludes : IEntityIncludes<Manager>
{
    /// <summary>
    /// A shared singleton instance to avoid unnecessary allocations.
    /// </summary>
    public static readonly ManagerIncludes Instance = new();

    /// <inheritdoc />
    public Func<IQueryable<Manager>, IQueryable<Manager>> Apply() =>
        q => q
            .Include(m => m.User)
            .Include(m => m.ManagerContactInfos).ThenInclude(mc => mc.ContactInfo)
            .Include(m => m.ManagerAttachments).ThenInclude(ma => ma.Attachment);
}
