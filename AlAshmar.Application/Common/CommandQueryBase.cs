namespace AlAshmar.Application.Common;

/// <summary>
/// Marker interface for all commands.
/// </summary>
public interface ICommand;

/// <summary>
/// Marker interface for all commands that return a result.
/// </summary>
public interface ICommand<TResult> : ICommand;

/// <summary>
/// Marker interface for all queries.
/// </summary>
public interface IQuery;

/// <summary>
/// Marker interface for all queries that return a result.
/// </summary>
public interface IQuery<TResult> : IQuery;
