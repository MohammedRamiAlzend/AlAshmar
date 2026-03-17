using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Forms;

public class Form : Entity<Guid>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public FormType FormType { get; set; } = FormType.Normal;

    public AudienceType Audience { get; set; } = AudienceType.Students;

    public Guid AccessToken { get; set; } = Guid.NewGuid();

    public int? TimerMinutes { get; set; }

    public bool IsActive { get; set; } = true;

    public bool AllowMultipleResponses { get; set; } = false;

    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }

    public Guid? CreatedByManagerId { get; set; }
    public Manager? CreatedByManager { get; set; }

    public Guid? CreatedByTeacherId { get; set; }
    public Teacher? CreatedByTeacher { get; set; }

    public Guid? HalaqaId { get; set; }
    public Halaqa? Halaqa { get; set; }

    public Guid? CourseId { get; set; }
    public Course? Course { get; set; }

    public string? PrimaryColor { get; set; }
    public string? BackgroundColor { get; set; }
    public string? FontFamily { get; set; }

    public ICollection<FormQuestion> Questions { get; set; } = new List<FormQuestion>();
    public ICollection<FormResponse> Responses { get; set; } = new List<FormResponse>();
}
