using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext context;
        public EfRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<T> GetById(int id)
        {
            return await context.Set<T>().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            context.Remove(entity);
            await context.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
