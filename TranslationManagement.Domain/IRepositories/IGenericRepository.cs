using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Domain.Repositories
{

    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetEntities();
        Task AddEntity(TEntity entity);
        Task AddEntityRange(IEnumerable<TEntity> entities);
        void AddEntityRangeSeed(IEnumerable<TEntity> entities);
        void UpdateEntity(TEntity entity);
        void RemoveEntity(TEntity entity);
        void RemoveEntityRange(IEnumerable<TEntity> entities);
        void UpdateEntityRange(IEnumerable<TEntity> entities);
        void DeleteEntity(TEntity entity);
        Task SaveChange();
    }
}