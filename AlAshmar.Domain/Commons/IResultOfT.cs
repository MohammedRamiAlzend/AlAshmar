namespace AlAshmar.Domain.Commons;

public interface IResult<out TValue> : IResult
{
    TValue? Value { get; }
}