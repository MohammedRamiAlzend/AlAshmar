namespace AlAshmar.Application.Repos.Includes;








public interface IEntityIncludes<TEntity>
{





    Func<IQueryable<TEntity>, IQueryable<TEntity>> Apply();
}
