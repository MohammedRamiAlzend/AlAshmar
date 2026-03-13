namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// A single question inside a <see cref="Form"/>.
/// </summary>
public class FormQuestion : Entity<Guid>
{
    public Guid FormId { get; set; }
    public Form? Form { get; set; }

    public string Text { get; set; } = null!;
    public string? Description { get; set; }

    public QuestionType QuestionType { get; set; } = QuestionType.ShortText;

    /// <summary>Display order within the form (1-based).</summary>
    public int Order { get; set; }

    public bool IsRequired { get; set; } = false;

    // ── Quiz-specific ──────────────────────────────────────────────────────────

    /// <summary>Points awarded for a correct answer (null for non-quiz forms).</summary>
    public int? Points { get; set; }

    // ── Navigation ─────────────────────────────────────────────────────────────

    public ICollection<FormQuestionOption> Options { get; set; } = new List<FormQuestionOption>();
    public ICollection<FormAnswer> Answers { get; set; } = new List<FormAnswer>();
}
