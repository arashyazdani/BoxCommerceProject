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

        public GenericRepository(FactoryContext context)
        {
            _context = context;
        }
        public async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(id, cancellationToken);
        }

        public async Task<T> SearchAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecification(spec).ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken)
        {
            return await ApplySpecification(spec).CountAsync(cancellationToken);
        }

        public IQueryable<IReadOnlyList<T>> GroupBy(Expression<Func<T, int>> spec)
        {
            try
            {
                return (IQueryable<IReadOnlyList<T>>)_context.Set<T>().GroupBy(spec);
            }
            catch
            {
                return null;
            }
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }
    }
}
