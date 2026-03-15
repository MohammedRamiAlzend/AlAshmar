namespace AlAshmar.Domain.ValueObjects;

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
            return ApplicationErrors.MissingRequiredField;

        if (!email.Contains("@") || !email.Contains("."))
            return ApplicationErrors.InvalidEmailFormat;

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}

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
            return ApplicationErrors.MissingRequiredField;

        if (name.Length > 100)
            return ApplicationErrors.InvalidInput;

        return new Name(name);
    }

    public static implicit operator string(Name name) => name.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
