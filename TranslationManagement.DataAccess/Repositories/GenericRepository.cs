using DataAccess.Databases;
using DataAccess.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DataAccess.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext context;
        private DbSet<TEntity> DbSet { get; }

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
            this.DbSet = context.Set<TEntity>();
        }
        public IQueryable<TEntity> GetEntities()
        {
            return DbSet.AsQueryable();
        }


        public async Task AddEntity(TEntity entity)
        {


            await DbSet.AddAsync(entity);
        }
        public async Task AddEntityRange(IEnumerable<TEntity> entities)
        {


            await DbSet.AddRangeAsync(entities);

        }
        public void RemoveEntity(TEntity entity)
        {

            UpdateEntity(entity);
        }
        public void DeleteEntity(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public void RemoveEntityRange(IEnumerable<TEntity> entities)
        {

            UpdateEntityRange(entities);
        }

        public void UpdateEntity(TEntity entity)
        {

            DbSet.Update(entity);
        }
        public void UpdateEntityRange(IEnumerable<TEntity> entities)
        {


            DbSet.UpdateRange(entities);
        }
        public async Task SaveChange()
        {
            await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context?.Dispose();
        }

        public void AddEntityRangeSeed(IEnumerable<TEntity> entities)
        {


            DbSet.AddRange(entities);
            context.SaveChanges();
        }
    }
}