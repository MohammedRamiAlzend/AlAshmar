using AlAshmar.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class FormAnswerIncludes : IEntityIncludes<FormAnswer>
{
    private readonly IReadOnlyList<Func<IQueryable<FormAnswer>, IQueryable<FormAnswer>>> _steps;

    private FormAnswerIncludes(IEnumerable<Func<IQueryable<FormAnswer>, IQueryable<FormAnswer>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly FormAnswerIncludes None = new([]);

    public static readonly FormAnswerIncludes Basic = None
        .WithQuestion();

    public static readonly FormAnswerIncludes Full = None
        .WithQuestion()
        .WithSelectedOptions();

    public static readonly FormAnswerIncludes Instance = Full;

    public FormAnswerIncludes WithQuestion() =>
        Add(q => q.Include(fa => fa.Question).ThenInclude(fq => fq!.Options));

    public FormAnswerIncludes WithSelectedOptions() =>
        Add(q => q.Include(fa => fa.SelectedOptions).ThenInclude(so => so.FormQuestionOption));

    public Func<IQueryable<FormAnswer>, IQueryable<FormAnswer>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private FormAnswerIncludes Add(Func<IQueryable<FormAnswer>, IQueryable<FormAnswer>> step) =>
        new(_steps.Append(step));
}
