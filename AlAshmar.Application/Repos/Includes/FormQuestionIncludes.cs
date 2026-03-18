using AlAshmar.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class FormQuestionIncludes : IEntityIncludes<FormQuestion>
{
    private readonly IReadOnlyList<Func<IQueryable<FormQuestion>, IQueryable<FormQuestion>>> _steps;

    private FormQuestionIncludes(IEnumerable<Func<IQueryable<FormQuestion>, IQueryable<FormQuestion>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly FormQuestionIncludes None = new([]);

    public static readonly FormQuestionIncludes Basic = None
        .WithOptions();

    public static readonly FormQuestionIncludes Full = None
        .WithOptions()
        .WithAnswers();

    public static readonly FormQuestionIncludes Instance = Full;

    public FormQuestionIncludes WithOptions() =>
        Add(q => q.Include(fq => fq.Options));

    public FormQuestionIncludes WithAnswers() =>
        Add(q => q.Include(fq => fq.Answers));

    public Func<IQueryable<FormQuestion>, IQueryable<FormQuestion>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private FormQuestionIncludes Add(Func<IQueryable<FormQuestion>, IQueryable<FormQuestion>> step) =>
        new(_steps.Append(step));
}
