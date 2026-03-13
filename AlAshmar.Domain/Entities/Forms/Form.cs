using AlAshmar.Domain.Entities.Academic;
using AlAshmar.Domain.Entities.Managers;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// Represents a form or quiz that can be filled out by students and/or teachers.
/// Access is controlled either by authentication or by a unique magic-link token.
/// </summary>
public class Form : Entity<Guid>
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    /// <summary>Indicates whether this is a plain form or a graded quiz.</summary>
    public FormType FormType { get; set; } = FormType.Normal;

    /// <summary>Who is allowed to respond to this form.</summary>
    public AudienceType Audience { get; set; } = AudienceType.Students;

    /// <summary>Unique token used to generate a magic-link for anonymous/direct access.</summary>
    public Guid AccessToken { get; set; } = Guid.NewGuid();

    /// <summary>Optional time limit in minutes (null = no limit).</summary>
    public int? TimerMinutes { get; set; }

    /// <summary>Whether the form is currently accepting responses.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Whether a single respondent is allowed to submit more than once.</summary>
    public bool AllowMultipleResponses { get; set; } = false;

    /// <summary>Optional window during which responses are accepted.</summary>
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }

    // ── Creator (one of these should be set) ──────────────────────────────────

    public Guid? CreatedByManagerId { get; set; }
    public Manager? CreatedByManager { get; set; }

    public Guid? CreatedByTeacherId { get; set; }
    public Teacher? CreatedByTeacher { get; set; }

    // ── Optional scope (for quizzes tied to a class or course/event) ──────────

    public Guid? HalaqaId { get; set; }
    public Halaqa? Halaqa { get; set; }

    public Guid? CourseId { get; set; }
    public Course? Course { get; set; }

    // ── Navigation ─────────────────────────────────────────────────────────────

    public ICollection<FormQuestion> Questions { get; set; } = new List<FormQuestion>();
    public ICollection<FormResponse> Responses { get; set; } = new List<FormResponse>();
}
