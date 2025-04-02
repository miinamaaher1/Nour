using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using XCourse.Infrastructure.Data;
using XCourse.Infrastructure.Repositories.Interfaces;

namespace XCourse.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly XCourseContext context;

        public BaseRepository(XCourseContext _context)
        {
            context = _context;
        }

        public T Get(int id)
        {
            return context.Set<T>().Find(id);
        }
        public async Task<T> GetAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        public IEnumerable<T> GetAll(string[]? includes = null, int? skip = null, int? take = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query.ToList();
        }
        public async Task<IEnumerable<T>> GetAllAsync(string[]? includes = null, int? skip = null, int? take = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public T Find(Expression<Func<T, bool>> expression, string[]? includes = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.SingleOrDefault(expression);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> expression, string[]? includes = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.SingleOrDefaultAsync(expression);
        }
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> expression, string[]? includes = null, int? skip = null, int? take = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query.Where(expression).ToList();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression, string[]? includes = null, int? skip = null, int? take = null)
        {
            IQueryable<T> query = context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.Where(expression).ToListAsync();
        }

        public T Add(T entity)
        {
            context.Set<T>().Add(entity);
            return entity;
        }
        public async Task<T> AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            return entity;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            context.Set<T>().AddRange(entities);
            return entities;
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public T Update(T entity)
        {
            context.Update(entity);
            return entity;
        }

        public void Attach(T entity)
        {
            context.Set<T>().Attach(entity);
        }
        public void AttachRange(IEnumerable<T> entities)
        {
            context.Set<T>().AttachRange(entities);
        }

        public void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            context.Set<T>().RemoveRange(entities);
        }
    }
}
