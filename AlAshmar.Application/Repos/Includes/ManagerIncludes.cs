using AlAshmar.Domain.Entities.Managers;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class ManagerIncludes : IEntityIncludes<Manager>
{
    private readonly IReadOnlyList<Func<IQueryable<Manager>, IQueryable<Manager>>> _steps;

    private ManagerIncludes(IEnumerable<Func<IQueryable<Manager>, IQueryable<Manager>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly ManagerIncludes None = new([]);

    public static readonly ManagerIncludes Basic = None
        .WithUser();

    public static readonly ManagerIncludes Full = None
        .WithUser()
        .WithContactInfos()
        .WithAttachments();

    public static readonly ManagerIncludes Instance = Full;

    public ManagerIncludes WithUser() =>
        Add(q => q.Include(m => m.User));

    public ManagerIncludes WithContactInfos() =>
        Add(q => q.Include(m => m.ManagerContactInfos).ThenInclude(mc => mc.ContactInfo));

    public ManagerIncludes WithAttachments() =>
        Add(q => q.Include(m => m.ManagerAttachments).ThenInclude(ma => ma.Attachment));

    public Func<IQueryable<Manager>, IQueryable<Manager>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private ManagerIncludes Add(Func<IQueryable<Manager>, IQueryable<Manager>> step) =>
        new(_steps.Append(step));
}
