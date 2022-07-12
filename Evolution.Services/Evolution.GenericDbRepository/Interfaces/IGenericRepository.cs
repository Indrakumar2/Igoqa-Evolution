using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evolution.GenericDbRepository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);

        Task<TEntity> AddAsync(TEntity entity);

        Task AddAsync(IList<TEntity> entities);

        void Add(IList<TEntity> entities);
        
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate,params Expression<Func<TEntity, object>>[] includes);

        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string[] includes);

        IQueryable<TEntity> GetAll();

        IQueryable<TType> Get<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TType>> select); //where TType : class;

        IQueryable<TEntity> Get<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> select);

        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);

        void Delete(TEntity entity);

        void Delete(IList<TEntity> entities);

        void Delete(Expression<Func<TEntity, bool>> predicate);

        void BulkDelete(Expression<Func<TEntity, bool>> predicate);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(IList<TEntity> entities);

        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);

        void Update(IList<TEntity> entities);
        
        void Update(TEntity entities, params Expression<Func<TEntity, object>>[] updatedProperties);

        void Update(IList<TEntity> entities, params Expression<Func<TEntity, object>>[] updatedProperties);

        Task UpdateAsync(TEntity entity);

        Task UpdateAsync(List<TEntity> entities);

        Task SaveAsync();

        Task<int> ForceSaveAsync();

        int ForceSave();

        bool AutoSave { get; set; }
    }
}
