using EFCore.BulkExtensions;
using Evolution.GenericDbRepository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Evolution.GenericDbRepository.Services
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DbContext _dbContext;

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private bool _autoSave = true;

        public bool AutoSave
        {
            get => this._autoSave;
            set => this._autoSave = value;
        }

        public DbContext Context
        {
            get => this._dbContext;
            set => this._dbContext = value;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsQueryable<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            //try
            //{
            _dbContext.Set<TEntity>().Add(entity);
            if (this.AutoSave)
                _dbContext.SaveChanges();
            return entity;
            //}
            //catch (Exception ex)
            //{ return entity; }
        }

        public void Add(IList<TEntity> entities)
        {
            //try
            //{
            _dbContext.Set<TEntity>().AddRange(entities);
            if (this.AutoSave)
                _dbContext.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(IList<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            if (this.AutoSave)
                _dbContext.SaveChanges();
        }

        public void Update(IList<TEntity> entities)
        {
            entities?.ToList().ForEach(entity =>
            {
                _dbContext.Set<TEntity>().Update(entity);
            });

            if (this.AutoSave)
                _dbContext.SaveChanges();
        }

        public Task UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (this.AutoSave)
                _dbContext.SaveChanges();
        }

        public void Delete(IList<TEntity> entities)
        {
            entities?.ToList().ForEach(entity =>
            {
                _dbContext.Set<TEntity>().Remove(entity);
            });

            if (this.AutoSave)
                _dbContext.SaveChanges();
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var results = this.FindBy(predicate);
            if (results != null && results.Count() > 0)
            {
                _dbContext.RemoveRange(results);
                if (this.AutoSave)
                    _dbContext.SaveChanges();
            }
        }
        public void BulkDelete(Expression<Func<TEntity, bool>> predicate)
        {
            var results = this.FindBy(predicate);
            if (results != null && results.Count() > 0)
            {
                _dbContext.BulkDelete(results.ToList());
                if (this.AutoSave)
                    _dbContext.SaveChanges();
            }
        }
         
        public Task DeleteAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IList<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(_dbContext.Set<TEntity>().Any(predicate));
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                return this.GetAll();
            else
                return _dbContext.Set<TEntity>().Where(predicate);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> result = FindBy(predicate);

            if (includes.Count() > 0)
                return includes.Aggregate(result, (current, include) => current.Include(include));

            return result;
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, string[] includes)
        {
            IQueryable<TEntity> result = FindBy(predicate);

            if (includes?.Count() > 0)
                return includes?.Aggregate(result, (current, include) => current.Include(include));

            return result;
        }

        public int ForceSave()
        {
            int success = -1;
            try
            {
                if (!this.AutoSave)
                    success = _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return success;
        }

        public Task<int> ForceSaveAsync()
        {
            throw new NotImplementedException();
        }
        public virtual void Update(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            //Ensure only modified fields are updated.
            var dbEntityEntry = _dbContext.Entry(entity);
            if (updatedProperties != null && updatedProperties.Any())
            {
                //update explicitly mentioned properties
                foreach (var property in updatedProperties)
                {
                    dbEntityEntry.Property(property).IsModified = true;
                }
                if (this.AutoSave)
                    _dbContext.SaveChanges();
            }
        }

        public virtual void Update(IList<TEntity> entities, params Expression<Func<TEntity, object>>[] updatedProperties)
        {
            entities?.ToList().ForEach(entity =>
            {
                var dbEntityEntry = _dbContext.Entry(entity);
                if (updatedProperties != null && updatedProperties.Any())
                {
                    foreach (var property in updatedProperties)
                    {
                        dbEntityEntry.Property(property).IsModified = true;
                    }
                }
            });
            if (this.AutoSave)
                _dbContext.SaveChanges();
        }

        //returns IQuerable anonymous
        public IQueryable<TType> Get<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TType>> select) //where TType : class
        {
            //_dbContext.Set<TEntity>().AsQueryable().Where(where).Select(select).ToList()
            return _dbContext.Set<TEntity>().AsQueryable().Where(where).Select(select);
        }

        //returns IQuerable Entity
        public IQueryable<TEntity> Get<TType>(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> select) 
        {
            //_dbContext.Set<TEntity>().AsQueryable().Where(where).Select(select).ToList()
            return _dbContext.Set<TEntity>().AsQueryable().Where(where).Select(select);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
