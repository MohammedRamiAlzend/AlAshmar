namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// The respondent's answer to a single <see cref="FormQuestion"/> within a
/// <see cref="FormResponse"/>.
/// </summary>
public class FormAnswer : Entity<Guid>
{
    public Guid ResponseId { get; set; }
    public FormResponse? Response { get; set; }

    public Guid QuestionId { get; set; }
    public FormQuestion? Question { get; set; }

    /// <summary>Plain-text answer for ShortText / LongText questions.</summary>
    public string? TextAnswer { get; set; }

    // ── Quiz grading ───────────────────────────────────────────────────────────

    /// <summary>Whether this answer was auto-graded as correct (null = not a quiz).</summary>
    public bool? IsCorrect { get; set; }

    /// <summary>Points awarded for this answer (null = not a quiz).</summary>
    public int? PointsAwarded { get; set; }

    // ── Navigation ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Selected options for MultipleChoice / Checkbox / Dropdown questions.
    /// </summary>
    public ICollection<FormAnswerSelectedOption> SelectedOptions { get; set; } = new List<FormAnswerSelectedOption>();
}
