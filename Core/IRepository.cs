using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Interfaces.IBusiness
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                string includeProperties = "");
        TEntity GetById(TKey id);
        void Insert(TEntity entityToAdd);
        void Update(TEntity entityToUpdate);
        void Delete(TKey id);
    }
}
