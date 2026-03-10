using MediatR;

namespace AlAshmar.Application.Common;

/// <summary>
/// Marker interface for all commands.
/// </summary>
public partial interface ICommand;

/// <summary>
/// Marker interface for all commands that return a result.
/// </summary>
public partial interface ICommand<TResult> : ICommand, IRequest<TResult>;

/// <summary>
/// Marker interface for all queries.
/// </summary>
public partial interface IQuery;

/// <summary>
/// Marker interface for all queries that return a result.
/// </summary>
public partial interface IQuery<TResult> : IQuery, IRequest<TResult>;
