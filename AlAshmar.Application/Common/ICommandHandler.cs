using AlAshmar.Domain.Commons;

namespace AlAshmar.Application.Common;

/// <summary>
/// Generic interface for handling commands.
/// </summary>
public interface ICommandHandler<in TCommand>
    where TCommand : ICommand<Result<Success>>
{
    Task<Result<Success>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic interface for handling commands with a result.
/// </summary>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic interface for handling queries.
/// </summary>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}
