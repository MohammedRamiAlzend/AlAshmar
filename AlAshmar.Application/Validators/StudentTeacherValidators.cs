using AlAshmar.Application.UseCases.Students.CreateStudent;
using AlAshmar.Application.UseCases.Students.UpdateStudent;
using AlAshmar.Application.UseCases.Teachers.CreateTeacher;
using AlAshmar.Application.UseCases.Teachers.UpdateTeacher;

namespace AlAshmar.Application.Validators;

/// <summary>
/// Validator for CreateStudentCommand.
/// Provides manual validation rules for student creation.
/// </summary>
public static class CreateStudentCommandValidator
{
    public static List<string> Validate(CreateStudentCommand command)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add("Name is required.");
        else if (command.Name.Length > 100)
            errors.Add("Name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.FatherName))
            errors.Add("Father name is required.");
        else if (command.FatherName.Length > 100)
            errors.Add("Father name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.MotherName))
            errors.Add("Mother name is required.");
        else if (command.MotherName.Length > 100)
            errors.Add("Mother name must not exceed 100 characters.");
        
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            if (!IsValidEmail(command.Email))
                errors.Add("Invalid email address.");
            else if (command.Email.Length > 256)
                errors.Add("Email must not exceed 256 characters.");
        }
        
        if (!string.IsNullOrWhiteSpace(command.NationalityNumber) && 
            command.NationalityNumber.Length > 50)
            errors.Add("Nationality number must not exceed 50 characters.");
        
        if (string.IsNullOrWhiteSpace(command.UserName))
            errors.Add("Username is required.");
        else if (command.UserName.Length > 50)
            errors.Add("Username must not exceed 50 characters.");
        
        if (string.IsNullOrWhiteSpace(command.Password))
            errors.Add("Password is required.");
        else if (command.Password.Length < 8)
            errors.Add("Password must be at least 8 characters long.");
        
        return errors;
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Validator for UpdateStudentCommand.
/// Provides manual validation rules for student updates.
/// </summary>
public static class UpdateStudentCommandValidator
{
    public static List<string> Validate(UpdateStudentCommand command)
    {
        var errors = new List<string>();
        
        if (command.Id == Guid.Empty)
            errors.Add("Student ID is required.");
        
        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add("Name is required.");
        else if (command.Name.Length > 100)
            errors.Add("Name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.FatherName))
            errors.Add("Father name is required.");
        else if (command.FatherName.Length > 100)
            errors.Add("Father name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.MotherName))
            errors.Add("Mother name is required.");
        else if (command.MotherName.Length > 100)
            errors.Add("Mother name must not exceed 100 characters.");
        
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            if (!IsValidEmail(command.Email))
                errors.Add("Invalid email address.");
            else if (command.Email.Length > 256)
                errors.Add("Email must not exceed 256 characters.");
        }
        
        if (!string.IsNullOrWhiteSpace(command.NationalityNumber) && 
            command.NationalityNumber.Length > 50)
            errors.Add("Nationality number must not exceed 50 characters.");
        
        return errors;
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Validator for CreateTeacherCommand.
/// Provides manual validation rules for teacher creation.
/// </summary>
public static class CreateTeacherCommandValidator
{
    public static List<string> Validate(CreateTeacherCommand command)
    {
        var errors = new List<string>();
        
        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add("Name is required.");
        else if (command.Name.Length > 100)
            errors.Add("Name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.FatherName))
            errors.Add("Father name is required.");
        else if (command.FatherName.Length > 100)
            errors.Add("Father name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.MotherName))
            errors.Add("Mother name is required.");
        else if (command.MotherName.Length > 100)
            errors.Add("Mother name must not exceed 100 characters.");
        
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            if (!IsValidEmail(command.Email))
                errors.Add("Invalid email address.");
            else if (command.Email.Length > 256)
                errors.Add("Email must not exceed 256 characters.");
        }
        
        if (!string.IsNullOrWhiteSpace(command.NationalityNumber) && 
            command.NationalityNumber.Length > 50)
            errors.Add("Nationality number must not exceed 50 characters.");
        
        if (string.IsNullOrWhiteSpace(command.UserName))
            errors.Add("Username is required.");
        else if (command.UserName.Length > 50)
            errors.Add("Username must not exceed 50 characters.");
        
        if (string.IsNullOrWhiteSpace(command.Password))
            errors.Add("Password is required.");
        else if (command.Password.Length < 8)
            errors.Add("Password must be at least 8 characters long.");
        
        return errors;
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

/// <summary>
/// Validator for UpdateTeacherCommand.
/// Provides manual validation rules for teacher updates.
/// </summary>
public static class UpdateTeacherCommandValidator
{
    public static List<string> Validate(UpdateTeacherCommand command)
    {
        var errors = new List<string>();
        
        if (command.Id == Guid.Empty)
            errors.Add("Teacher ID is required.");
        
        if (string.IsNullOrWhiteSpace(command.Name))
            errors.Add("Name is required.");
        else if (command.Name.Length > 100)
            errors.Add("Name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.FatherName))
            errors.Add("Father name is required.");
        else if (command.FatherName.Length > 100)
            errors.Add("Father name must not exceed 100 characters.");
        
        if (string.IsNullOrWhiteSpace(command.MotherName))
            errors.Add("Mother name is required.");
        else if (command.MotherName.Length > 100)
            errors.Add("Mother name must not exceed 100 characters.");
        
        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            if (!IsValidEmail(command.Email))
                errors.Add("Invalid email address.");
            else if (command.Email.Length > 256)
                errors.Add("Email must not exceed 256 characters.");
        }
        
        if (!string.IsNullOrWhiteSpace(command.NationalityNumber) && 
            command.NationalityNumber.Length > 50)
            errors.Add("Nationality number must not exceed 50 characters.");
        
        return errors;
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
