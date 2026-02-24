using AlAshmar.Domain.ValueObjects;

namespace AlAshmar.Domain.ValueObjects;

/// <summary>
/// Value object representing an email address.
/// </summary>
public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return new Error("INVALID_EMAIL", "Email cannot be empty", ErrorKind.Validation);

        // Simple email validation
        if (!email.Contains("@") || !email.Contains("."))
            return new Error("INVALID_EMAIL", "Invalid email format", ErrorKind.Validation);

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}

/// <summary>
/// Value object representing a person's name.
/// </summary>
public class Name : ValueObject
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return new Error("INVALID_NAME", "Name cannot be empty", ErrorKind.Validation);

        if (name.Length > 100)
            return new Error("INVALID_NAME", "Name must not exceed 100 characters", ErrorKind.Validation);

        return new Name(name);
    }

    public static implicit operator string(Name name) => name.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
