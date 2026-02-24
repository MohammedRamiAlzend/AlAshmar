namespace AlAshmar.Application.Validators;

/// <summary>
/// Validator for CreateStudentCommand.
/// </summary>
public class CreateStudentCommandValidator
{
    public CreateStudentCommandValidator()
    {
        // TODO: Add FluentValidation rules when package is installed
        // RuleFor(x => x.Name)
        //     .NotEmpty().WithMessage("Name is required.")
        //     .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
        // RuleFor(x => x.FatherName)
        //     .NotEmpty().WithMessage("Father name is required.");
        // RuleFor(x => x.MotherName)
        //     .NotEmpty().WithMessage("Mother name is required.");
        // RuleFor(x => x.Email)
        //     .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
        //     .WithMessage("Invalid email address.");
    }
}

/// <summary>
/// Validator for UpdateStudentCommand.
/// </summary>
public class UpdateStudentCommandValidator
{
    public UpdateStudentCommandValidator()
    {
        // TODO: Add FluentValidation rules when package is installed
    }
}

/// <summary>
/// Validator for CreateTeacherCommand.
/// </summary>
public class CreateTeacherCommandValidator
{
    public CreateTeacherCommandValidator()
    {
        // TODO: Add FluentValidation rules when package is installed
    }
}

/// <summary>
/// Validator for UpdateTeacherCommand.
/// </summary>
public class UpdateTeacherCommandValidator
{
    public UpdateTeacherCommandValidator()
    {
        // TODO: Add FluentValidation rules when package is installed
    }
}
