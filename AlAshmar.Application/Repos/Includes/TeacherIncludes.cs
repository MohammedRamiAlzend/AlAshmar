using AlAshmar.Domain.Entities.Teachers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class TeacherIncludes : IEntityIncludes<Teacher>
{
    private readonly IReadOnlyList<Func<IQueryable<Teacher>, IQueryable<Teacher>>> _steps;

    private TeacherIncludes(IEnumerable<Func<IQueryable<Teacher>, IQueryable<Teacher>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly TeacherIncludes None = new([]);

    public static readonly TeacherIncludes Basic = None
        .WithRelatedUser();

    public static readonly TeacherIncludes Full = None
        .WithRelatedUser()
        .WithContactInfos()
        .WithAttachments()
        .WithEnrollments()
        .WithGivenPoints();

    public static readonly TeacherIncludes Instance = Full;

    public TeacherIncludes WithRelatedUser() =>
        Add(q => q.Include(t => t.RelatedUser));

    public TeacherIncludes WithContactInfos() =>
        Add(q => q.Include(t => t.TeacherContactInfos).ThenInclude(tc => tc.ContactInfo));

    public TeacherIncludes WithAttachments() =>
        Add(q => q.Include(t => t.TeacherAttachments).ThenInclude(ta => ta.Attachment));

    public TeacherIncludes WithEnrollments() =>
        Add(q => q.Include(t => t.ClassTeacherEnrollments));

    public TeacherIncludes WithGivenPoints() =>
        Add(q => q.Include(t => t.GivenPoints).ThenInclude(p => p.Category));

    public Func<IQueryable<Teacher>, IQueryable<Teacher>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private TeacherIncludes Add(Func<IQueryable<Teacher>, IQueryable<Teacher>> step) =>
        new(_steps.Append(step));
}
