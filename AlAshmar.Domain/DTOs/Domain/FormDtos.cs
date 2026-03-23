using AlAshmar.Domain.Entities.Forms;

namespace AlAshmar.Domain.DTOs.Domain;

public record FormDto(
    Guid Id,
    string Title,
    string? Description,
    FormType FormType,
    AudienceType Audience,
    Guid AccessToken,
    int? TimerMinutes,
    bool IsActive,
    bool AllowMultipleResponses,
    DateTime? StartsAt,
    DateTime? EndsAt,
    Guid? CreatedByManagerId,
    Guid? CreatedByTeacherId,
    Guid? HalaqaId,
    Guid? CourseId,
    string? PrimaryColor,
    string? BackgroundColor,
    string? FontFamily,
    List<FormQuestionDto> Questions
);

public record FormQuestionDto(
    Guid Id,
    Guid FormId,
    string Text,
    string? Description,
    QuestionType QuestionType,
    int Order,
    bool IsRequired,
    int? Points,
    int ColumnSpan,
    string? LabelColor,
    int? FontSize,
    string? FontFamily,
    List<FormQuestionOptionDto> Options
);

public record FormQuestionOptionDto(
    Guid Id,
    Guid QuestionId,
    string Text,
    int Order,
    bool IsCorrect
);

public record FormResponseDto(
    Guid Id,
    Guid FormId,
    Guid? RespondedByStudentId,
    Guid? RespondedByTeacherId,
    DateTime SubmittedAt,
    int? TimeSpentSeconds,
    bool IsCompleted,
    int? TotalScore,
    List<FormAnswerDto> Answers
);

public record FormAnswerDto(
    Guid Id,
    Guid ResponseId,
    Guid QuestionId,
    string? TextAnswer,
    bool? IsCorrect,
    int? PointsAwarded,
    List<Guid> SelectedOptionIds
);

public record CreateFormDto(
    string Title,
    string? Description,
    FormType FormType,
    AudienceType Audience,
    int? TimerMinutes,
    bool IsActive,
    bool AllowMultipleResponses,
    DateTime? StartsAt,
    DateTime? EndsAt,
    Guid? CreatedByManagerId,
    Guid? CreatedByTeacherId,
    Guid? HalaqaId,
    Guid? CourseId,
    string? PrimaryColor,
    string? BackgroundColor,
    string? FontFamily
);

public record UpdateFormDto(
    string Title,
    string? Description,
    FormType FormType,
    AudienceType Audience,
    int? TimerMinutes,
    bool IsActive,
    bool AllowMultipleResponses,
    DateTime? StartsAt,
    DateTime? EndsAt,
    Guid? HalaqaId,
    Guid? CourseId,
    string? PrimaryColor,
    string? BackgroundColor,
    string? FontFamily
);

public record CreateFormQuestionDto(
    Guid FormId,
    string Text,
    string? Description,
    QuestionType QuestionType,
    int Order,
    bool IsRequired,
    int? Points,
    int ColumnSpan,
    string? LabelColor,
    int? FontSize,
    string? FontFamily,
    List<CreateFormQuestionOptionDto> Options
);

public record UpdateFormQuestionDto(
    string Text,
    string? Description,
    QuestionType QuestionType,
    int Order,
    bool IsRequired,
    int? Points,
    int ColumnSpan,
    string? LabelColor,
    int? FontSize,
    string? FontFamily
);

public record CreateFormQuestionOptionDto(
    Guid QuestionId,
    string Text,
    int Order,
    bool IsCorrect
);

public record UpdateFormQuestionOptionDto(
    string Text,
    int Order,
    bool IsCorrect
);

public record SubmitFormResponseDto(
    Guid FormId,
    Guid? RespondedByStudentId,
    Guid? RespondedByTeacherId,
    int? TimeSpentSeconds,
    List<SubmitFormAnswerDto> Answers
);

public record SubmitFormAnswerDto(
    Guid QuestionId,
    string? TextAnswer,
    List<Guid> SelectedOptionIds
);
