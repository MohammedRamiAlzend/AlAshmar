namespace AlAshmar.Domain.Entities.Forms;

public enum FormType
{
    Normal,
    Quiz
}

public enum QuestionType
{
    ShortText,
    LongText,
    MultipleChoice,
    Checkbox,
    Dropdown
}

public enum AudienceType
{
    Students,
    Teachers,
    Both
}
