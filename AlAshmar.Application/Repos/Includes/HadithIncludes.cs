using AlAshmar.Domain.Entities.Academic;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class HadithIncludes : IEntityIncludes<Hadith>
{
    private readonly IReadOnlyList<Func<IQueryable<Hadith>, IQueryable<Hadith>>> _steps;

    private HadithIncludes(IEnumerable<Func<IQueryable<Hadith>, IQueryable<Hadith>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly HadithIncludes None = new([]);

    public static readonly HadithIncludes Full = None
        .WithBook();

    public static readonly HadithIncludes Instance = Full;

    public HadithIncludes WithBook() =>
        Add(q => q.Include(h => h.Book));

    public Func<IQueryable<Hadith>, IQueryable<Hadith>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private HadithIncludes Add(Func<IQueryable<Hadith>, IQueryable<Hadith>> step) =>
        new(_steps.Append(step));
}
