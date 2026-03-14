namespace AlAshmar.Domain.Entities.Forms;





public class FormAnswer : Entity<Guid>
{
    public Guid ResponseId { get; set; }
    public FormResponse? Response { get; set; }

    public Guid QuestionId { get; set; }
    public FormQuestion? Question { get; set; }


    public string? TextAnswer { get; set; }




    public bool? IsCorrect { get; set; }


    public int? PointsAwarded { get; set; }






    public ICollection<FormAnswerSelectedOption> SelectedOptions { get; set; } = new List<FormAnswerSelectedOption>();
}
