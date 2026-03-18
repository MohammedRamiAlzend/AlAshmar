using AlAshmar.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class FormIncludes : IEntityIncludes<Form>
{
    private readonly IReadOnlyList<Func<IQueryable<Form>, IQueryable<Form>>> _steps;

    private FormIncludes(IEnumerable<Func<IQueryable<Form>, IQueryable<Form>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly FormIncludes None = new([]);

    public static readonly FormIncludes Basic = None
        .WithQuestions();

    public static readonly FormIncludes Full = None
        .WithCreatedByManager()
        .WithCreatedByTeacher()
        .WithHalaqa()
        .WithCourse()
        .WithQuestions()
        .WithResponses();

    public static readonly FormIncludes Instance = Full;

    public FormIncludes WithCreatedByManager() =>
        Add(q => q.Include(f => f.CreatedByManager));

    public FormIncludes WithCreatedByTeacher() =>
        Add(q => q.Include(f => f.CreatedByTeacher));

    public FormIncludes WithHalaqa() =>
        Add(q => q.Include(f => f.Halaqa));

    public FormIncludes WithCourse() =>
        Add(q => q.Include(f => f.Course));

    public FormIncludes WithQuestions() =>
        Add(q => q.Include(f => f.Questions).ThenInclude(fq => fq.Options));

    public FormIncludes WithResponses() =>
        Add(q => q.Include(f => f.Responses));

    public Func<IQueryable<Form>, IQueryable<Form>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private FormIncludes Add(Func<IQueryable<Form>, IQueryable<Form>> step) =>
        new(_steps.Append(step));
}
