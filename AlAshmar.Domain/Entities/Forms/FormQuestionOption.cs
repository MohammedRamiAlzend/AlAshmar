namespace AlAshmar.Domain.Entities.Forms;





public class FormQuestionOption : Entity<Guid>
{
    public Guid QuestionId { get; set; }
    public FormQuestion? Question { get; set; }

    public string Text { get; set; } = null!;


    public int Order { get; set; }




    public bool IsCorrect { get; set; } = false;
}
