using Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<T> GetByIdAsync(object id);

        Task<T> SearchAsync(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec);

        Task InsertAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task DeleteAsync(object id);

        Task AddRangeAsync(IEnumerable<T> entities);

        void RemoveRange(IEnumerable<T> entities);

        Task<int> CountAsync(ISpecification<T> spec);

    }
}
