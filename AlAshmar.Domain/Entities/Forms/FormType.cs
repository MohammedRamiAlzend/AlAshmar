namespace AlAshmar.Domain.Entities.Forms;

/// <summary>
/// Indicates whether a form is a plain information-gathering form or a graded quiz.
/// </summary>
public enum FormType
{
    Normal,
    Quiz
}

/// <summary>
/// Defines the type of input expected from a respondent for a single question.
/// </summary>
public enum QuestionType
{
    ShortText,
    LongText,
    MultipleChoice,
    Checkbox,
    Dropdown
}

/// <summary>
/// Defines who the form/quiz is directed at.
/// </summary>
public enum AudienceType
{
    Students,
    Teachers,
    Both
}
