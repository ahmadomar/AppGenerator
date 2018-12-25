using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace AppsGenerator.Models.Repositories
{
    interface IGenericRepository<TEntity>
    {
        TEntity GetById(int id);
        List<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        TEntity FindFirst(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        void Edit(TEntity entity);
        void Insert(TEntity entity);
        void Delete(TEntity entity);

        void Dispose();
    }
}