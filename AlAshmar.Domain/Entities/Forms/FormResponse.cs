using AlAshmar.Domain.Entities.Students;
using AlAshmar.Domain.Entities.Teachers;

namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// Represents a single submission (response) to a <see cref="Form"/>.
/// </summary>
public class FormResponse : Entity<Guid>
{
    public Guid FormId { get; set; }
    public Form? Form { get; set; }

    // ── Respondent (one of these should be set) ────────────────────────────────

    public Guid? RespondedByStudentId { get; set; }
    public Student? RespondedByStudent { get; set; }

    public Guid? RespondedByTeacherId { get; set; }
    public Teacher? RespondedByTeacher { get; set; }

    // ── Timing ─────────────────────────────────────────────────────────────────

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Seconds the respondent spent on the form (tracked client-side).</summary>
    public int? TimeSpentSeconds { get; set; }

    public bool IsCompleted { get; set; } = true;

    // ── Quiz scoring ───────────────────────────────────────────────────────────

    /// <summary>Total score achieved (null for non-quiz forms).</summary>
    public int? TotalScore { get; set; }

    // ── Navigation ─────────────────────────────────────────────────────────────

    public ICollection<FormAnswer> Answers { get; set; } = new List<FormAnswer>();
}
