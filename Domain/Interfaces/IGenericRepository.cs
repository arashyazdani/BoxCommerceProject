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
        Task<T> GetByIdAsync(object id, CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<T>> SearchAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default(CancellationToken));

        Task InsertAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        void Update(T entity);

        void Delete(T entity);

        Task DeleteAsync(object id);

        Task AddRangeAsync(IEnumerable<T> entities);

        void RemoveRange(IEnumerable<T> entities);

        IQueryable<IReadOnlyList<T>> GroupBy(Expression<Func<T, int>> spec);

        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default(CancellationToken));

    }
}
