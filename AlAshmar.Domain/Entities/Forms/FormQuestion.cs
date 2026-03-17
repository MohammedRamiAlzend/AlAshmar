namespace AlAshmar.Domain.Entities.Forms;

public class FormQuestion : Entity<Guid>
{
    public Guid FormId { get; set; }
    public Form? Form { get; set; }

    public string Text { get; set; } = null!;
    public string? Description { get; set; }

    public QuestionType QuestionType { get; set; } = QuestionType.ShortText;

    public int Order { get; set; }

    public bool IsRequired { get; set; } = false;

    public int? Points { get; set; }

    public int ColumnSpan { get; set; } = 12;
    public string? LabelColor { get; set; }
    public int? FontSize { get; set; }
    public string? FontFamily { get; set; }

    public ICollection<FormQuestionOption> Options { get; set; } = new List<FormQuestionOption>();
    public ICollection<FormAnswer> Answers { get; set; } = new List<FormAnswer>();
}
