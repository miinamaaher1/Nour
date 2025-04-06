using System.Linq.Expressions;

namespace XCourse.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        Task<T> GetAsync(int id);
        IEnumerable<T> GetAll(string[]? includes = null, int? skip = null, int? take = null);
        Task<IEnumerable<T>> GetAllAsync(string[]? includes = null, int? skip = null, int? take = null);

        T Find(Expression<Func<T, bool>> expression, string[]? includes = null);
        Task<T> FindAsync(Expression<Func<T, bool>> expression, string[]? includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> expression, string[]? includes = null, int? skip = null, int? take = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression, string[]? includes = null, int? skip = null, int? take = null);

        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        T Update(T entity);

        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
