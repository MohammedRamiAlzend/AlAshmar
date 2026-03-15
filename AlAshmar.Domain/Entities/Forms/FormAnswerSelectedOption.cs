namespace AlAshmar.Domain.Entities.Forms;

public class FormAnswerSelectedOption
{
    public Guid FormAnswerId { get; set; }
    public FormAnswer? FormAnswer { get; set; }

    public Guid FormQuestionOptionId { get; set; }
    public FormQuestionOption? FormQuestionOption { get; set; }
}
