
namespace AlAshmar.Application.Common;




public partial interface IQuery;




public partial interface IQuery<TResult> : IQuery, IRequest<TResult>;
