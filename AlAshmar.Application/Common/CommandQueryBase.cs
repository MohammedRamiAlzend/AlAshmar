
namespace AlAshmar.Application.Common;




public partial interface ICommand;




public partial interface ICommand<TResult> : ICommand, IRequest<TResult>;




public partial interface IQuery;




public partial interface IQuery<TResult> : IQuery, IRequest<TResult>;
