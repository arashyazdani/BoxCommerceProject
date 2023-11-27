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
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<T>> SearchAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<T?> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken = default(CancellationToken));

        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default(CancellationToken));

        Task InsertAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        void Update(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateAsync(T entity, CancellationToken cancellationToken = default(CancellationToken));

        void Delete(T entity, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteAsync(object id, CancellationToken cancellationToken = default(CancellationToken));

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        void RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<IGrouping<TKey, T>>> GroupByAsync<TKey>(Expression<Func<T, TKey>> keySelector, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default(CancellationToken));

    }
}
