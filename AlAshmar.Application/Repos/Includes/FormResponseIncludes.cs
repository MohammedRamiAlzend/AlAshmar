using AlAshmar.Domain.Entities.Forms;
using Microsoft.EntityFrameworkCore;

namespace AlAshmar.Application.Repos.Includes;

public sealed class FormResponseIncludes : IEntityIncludes<FormResponse>
{
    private readonly IReadOnlyList<Func<IQueryable<FormResponse>, IQueryable<FormResponse>>> _steps;

    private FormResponseIncludes(IEnumerable<Func<IQueryable<FormResponse>, IQueryable<FormResponse>>> steps)
    {
        _steps = steps.ToList();
    }

    public static readonly FormResponseIncludes None = new([]);

    public static readonly FormResponseIncludes Basic = None
        .WithAnswers();

    public static readonly FormResponseIncludes Full = None
        .WithForm()
        .WithRespondedByStudent()
        .WithRespondedByTeacher()
        .WithAnswers();

    public static readonly FormResponseIncludes Instance = Full;

    public FormResponseIncludes WithForm() =>
        Add(q => q.Include(fr => fr.Form));

    public FormResponseIncludes WithRespondedByStudent() =>
        Add(q => q.Include(fr => fr.RespondedByStudent));

    public FormResponseIncludes WithRespondedByTeacher() =>
        Add(q => q.Include(fr => fr.RespondedByTeacher));

    public FormResponseIncludes WithAnswers() =>
        Add(q => q.Include(fr => fr.Answers).ThenInclude(a => a.SelectedOptions));

    public Func<IQueryable<FormResponse>, IQueryable<FormResponse>> Apply() =>
        q => _steps.Aggregate(q, (current, step) => step(current));

    private FormResponseIncludes Add(Func<IQueryable<FormResponse>, IQueryable<FormResponse>> step) =>
        new(_steps.Append(step));
}
