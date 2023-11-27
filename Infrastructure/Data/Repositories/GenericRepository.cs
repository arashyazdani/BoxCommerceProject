using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {

        private readonly FactoryContext _context;

        private const EntityEntryState ModifiedState = EntityEntryState.Modified;

        public GenericRepository(FactoryContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null.");
            }

            return await _context.Set<T>().FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);
        }

        public async Task<IList<T>> SearchAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");
            }

            return await  _context.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec), "Spec cannot be null.");
            }

            return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec), "Spec cannot be null.");
            }

            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public void Update(T entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            _context.Entry(entity).State = ConvertToEntityState(ModifiedState);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            _context.Entry(entity).State = ConvertToEntityState(ModifiedState);
            await Task.CompletedTask; 
        }

        public void Delete(T entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }

            _context.Set<T>().Remove(entity);
        }

        public async Task DeleteAsync(object id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null.");
            }

            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                var entry = _context.Entry(entity);
                entry.State = EntityState.Deleted;
            }
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null.");
            }

            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void RemoveRange(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities), "Entities cannot be null.");
            }

            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec), "Spec cannot be null.");
            }

            return await ApplySpecification(spec).CountAsync(cancellationToken);
        }

        public async Task<List<IGrouping<TKey, T>>> GroupByAsync<TKey>(Expression<Func<T, TKey>> keySelector, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (keySelector == null)
            {
                throw new ArgumentNullException(nameof(keySelector), "KeySelector cannot be null.");
            }

            var groupedData = await _context.Set<T>().GroupBy(keySelector).ToListAsync(cancellationToken);
            return groupedData;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec), "Spec cannot be null.");
            }

            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        private static EntityState ConvertToEntityState(EntityEntryState entryState)
        {
            return entryState switch
            {
                EntityEntryState.Unchanged => EntityState.Unchanged,
                EntityEntryState.Added => EntityState.Added,
                EntityEntryState.Modified => EntityState.Modified,
                EntityEntryState.Deleted => EntityState.Deleted,
                _ => throw new ArgumentOutOfRangeException(nameof(entryState), entryState, null)
            };
        }

        public enum EntityEntryState
        {
            Unchanged,
            Added,
            Modified,
            Deleted
        }
    }
}
