global using System.Collections.Generic;

namespace AlAshmar.Domain.Commons;

public interface IResult
{
    List<Error>? Errors { get; }
    bool IsSuccess { get; }
}
