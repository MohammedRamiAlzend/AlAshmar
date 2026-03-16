using AlAshmar.Domain.Entities.Students;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class StudentIncludes : IEntityIncludes<Student>
{
    private readonly IReadOnlyList<Func<IQueryable<Student>, IQueryable<Student>>> _steps;

    private StudentIncludes(IEnumerable<Func<IQueryable<Student>, IQueryable<Student>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly StudentIncludes None = new([]);

    public static readonly StudentIncludes Basic = None
        .WithUser();

    public static readonly StudentIncludes Full = None
        .WithUser()
        .WithContactInfos()
        .WithAttachments()
        .WithHadiths()
        .WithQuraanPages()
        .WithClassEventsPoints()
        .WithPoints();

    public static readonly StudentIncludes Instance = Full;

    public StudentIncludes WithUser() =>
        Add(q => q.Include(s => s.User));

    public StudentIncludes WithContactInfos() =>
        Add(q => q.Include(s => s.StudentContactInfos).ThenInclude(sc => sc.ContactInfo));

    public StudentIncludes WithAttachments() =>
        Add(q => q.Include(s => s.StudentAttachments).ThenInclude(sa => sa.Attachment).ThenInclude(a => a.Extension));

    public StudentIncludes WithHadiths() =>
        Add(q => q.Include(s => s.StudentHadiths).ThenInclude(h => h.Hadith).ThenInclude(h => h.Book));

    public StudentIncludes WithQuraanPages() =>
        Add(q => q.Include(s => s.StudentQuraanPages));

    public StudentIncludes WithClassEventsPoints() =>
        Add(q => q.Include(s => s.StudentClassEventsPoints));

    public StudentIncludes WithPoints() =>
        Add(q => q.Include(s => s.Points).ThenInclude(p => p.Category));

    public Func<IQueryable<Student>, IQueryable<Student>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private StudentIncludes Add(Func<IQueryable<Student>, IQueryable<Student>> step) =>
        new(_steps.Append(step));
}
