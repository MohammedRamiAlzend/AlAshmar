using AlAshmar.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class AttachmentIncludes : IEntityIncludes<Attachment>
{
    private readonly IReadOnlyList<Func<IQueryable<Attachment>, IQueryable<Attachment>>> _steps;

    private AttachmentIncludes(IEnumerable<Func<IQueryable<Attachment>, IQueryable<Attachment>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly AttachmentIncludes None = new([]);

    public static readonly AttachmentIncludes Full = None
        .WithExtension();

    public static readonly AttachmentIncludes Instance = Full;

    public AttachmentIncludes WithExtension() =>
        Add(q => q.Include(a => a.Extension));

    public Func<IQueryable<Attachment>, IQueryable<Attachment>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private AttachmentIncludes Add(Func<IQueryable<Attachment>, IQueryable<Attachment>> step) =>
        new(_steps.Append(step));
}
