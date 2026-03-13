namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// One selectable option for a <see cref="FormQuestion"/> of type
/// MultipleChoice, Checkbox, or Dropdown.
/// </summary>
public class FormQuestionOption : Entity<Guid>
{
    public Guid QuestionId { get; set; }
    public FormQuestion? Question { get; set; }

    public string Text { get; set; } = null!;

    /// <summary>Display order within the question (1-based).</summary>
    public int Order { get; set; }

    /// <summary>
    /// Marks this option as the correct answer (used for auto-grading quizzes).
    /// </summary>
    public bool IsCorrect { get; set; } = false;
}
