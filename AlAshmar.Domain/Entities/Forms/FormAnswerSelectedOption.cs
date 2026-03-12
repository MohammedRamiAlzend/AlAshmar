namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// Junction entity recording which <see cref="FormQuestionOption"/> was
/// selected in a particular <see cref="FormAnswer"/>.
/// </summary>
public class FormAnswerSelectedOption
{
    public Guid FormAnswerId { get; set; }
    public FormAnswer? FormAnswer { get; set; }

    public Guid FormQuestionOptionId { get; set; }
    public FormQuestionOption? FormQuestionOption { get; set; }
}
