using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Forms;




public class FormResponse : Entity<Guid>
{
    public Guid FormId { get; set; }
    public Form? Form { get; set; }



    public Guid? RespondedByStudentId { get; set; }
    public Student? RespondedByStudent { get; set; }

    public Guid? RespondedByTeacherId { get; set; }
    public Teacher? RespondedByTeacher { get; set; }



    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;


    public int? TimeSpentSeconds { get; set; }

    public bool IsCompleted { get; set; } = true;




    public int? TotalScore { get; set; }



    public ICollection<FormAnswer> Answers { get; set; } = new List<FormAnswer>();
}
