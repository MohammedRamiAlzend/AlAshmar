using AlAshmar.Domain.Entities.Students;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

/// <summary>
/// Centralizes all eager-loading includes for the <see cref="Student"/> entity.
/// Add new navigation property includes here once and they will be applied automatically
/// across all query methods (GetAllAsync, GetPagedAsync, GetAsync, etc.).
/// </summary>
public class StudentIncludes : IEntityIncludes<Student>
{
    /// <summary>
    /// A shared singleton instance to avoid unnecessary allocations.
    /// </summary>
    public static readonly StudentIncludes Instance = new();

    /// <inheritdoc />
    public Func<IQueryable<Student>, IQueryable<Student>> Apply() =>
        q => q
            .Include(s => s.User)
            .Include(s => s.StudentContactInfos).ThenInclude(sc => sc.ContactInfo)
            .Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment).ThenInclude(a => a.Extention)
            .Include(s => s.StudentHadiths).ThenInclude(h => h.Hadith).ThenInclude(h => h.Book)
            .Include(s => s.StudentQuraanPages)
            .Include(s => s.StudentClassEventsPoints)
            .Include(s => s.Points).ThenInclude(p => p.Category);
}
