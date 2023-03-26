using EFCore.BulkExtensions;
using Infrastracture.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    /// <summary>
    /// Class GenericRepository.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    /// <seealso cref="Infrastructure.Core.IRepository{TEntity}" />
    public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
    {
        #region Fields

        /// <summary>
        /// The database context
        /// </summary>
        protected DbContext _dbContext;
        /// <summary>
        /// The database set
        /// </summary>
        protected readonly DbSet<TEntity> _dbSet;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity, TModel}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <exception cref="ArgumentNullException">dbContext</exception>
        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        #endregion

        #region Methods

        #region Create

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Create(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                return entity;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Create(params TEntity[] entities) => _dbSet.AddRange(entities);

        /// <summary>
        /// Creates a range of entities synchronously.
        /// </summary>
        /// <param name="entities">The entities to insert.</param>
        public virtual void Create(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        #endregion

        #region Update

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(TEntity entity, object entityID)
        {
            var entry = _dbSet.Find(entityID);
            _dbContext.Entry(entry).CurrentValues.SetValues(entity);
        }

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(params TEntity[] entities) => _dbSet.UpdateRange(entities);

        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the entity by the specified primary key.
        /// </summary>
        /// <param name="id">The primary key value.</param>
        public virtual void Delete(object id)
        {
            // using a stub entity to mark for deletion
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = _dbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                _dbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = _dbSet.Find(id);
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Delete(params TEntity[] entities) => _dbSet.RemoveRange(entities);

        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void Delete(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

        #endregion

        #region Count

        /// <summary>
        /// Gets the count based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.Count();
            }
            else
            {
                return _dbSet.Count(predicate);
            }
        }

        /// <summary>
        /// Gets async the count based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.CountAsync();
            }
            else
            {
                return await _dbSet.CountAsync(predicate);
            }
        }

        /// <summary>
        /// Gets the long count based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int64.</returns>
        public virtual long LongCount(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbSet.LongCount();
            }
            else
            {
                return _dbSet.LongCount(predicate);
            }
        }

        /// <summary>
        /// Gets async the long count based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        public virtual async Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return await _dbSet.LongCountAsync();
            }
            else
            {
                return await _dbSet.LongCountAsync(predicate);
            }
        }

        #endregion

        #region Aggregate

        /// <summary>
        /// Gets the max based on a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual T Max<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null)
        {
            if (predicate == null)
                return _dbSet.Max(selector);
            else
                return _dbSet.Where(predicate).Max(selector);
        }

        /// <summary>
        /// Gets the async max based on a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual async Task<T> MaxAsync<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null)
        {
            if (predicate == null)
                return await _dbSet.MaxAsync(selector);
            else
                return await _dbSet.Where(predicate).MaxAsync(selector);
        }

        /// <summary>
        /// Gets the min based on a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual T Min<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null)
        {
            if (predicate == null)
                return _dbSet.Min(selector);
            else
                return _dbSet.Where(predicate).Min(selector);
        }

        /// <summary>
        /// Gets the async min based on a predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual async Task<T> MinAsync<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null)
        {
            if (predicate == null)
                return await _dbSet.MinAsync(selector);
            else
                return await _dbSet.Where(predicate).MinAsync(selector);
        }

        /// <summary>
        /// Gets the average based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual decimal Average(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null)
        {
            if (predicate == null)
                return _dbSet.Average(selector);
            else
                return _dbSet.Where(predicate).Average(selector);
        }

        /// <summary>
        /// Gets the async average based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual async Task<decimal> AverageAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null)
        {
            if (predicate == null)
                return await _dbSet.AverageAsync(selector);
            else
                return await _dbSet.Where(predicate).AverageAsync(selector);
        }

        /// <summary>
        /// Gets the sum based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual decimal Sum(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null)
        {
            if (predicate == null)
                return _dbSet.Sum(selector);
            else
                return _dbSet.Where(predicate).Sum(selector);
        }

        /// <summary>
        /// Gets the async sum based on a predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>decimal</returns>
        public virtual async Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null)
        {
            if (predicate == null)
                return await _dbSet.SumAsync(selector);
            else
                return await _dbSet.Where(predicate).SumAsync(selector);
        }

        /// <summary>
        /// Gets the exists based on a predicate.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Exists(Expression<Func<TEntity, bool>> selector = null)
        {
            if (selector == null)
            {
                return _dbSet.Any();
            }
            else
            {
                return _dbSet.Any(selector);
            }
        }
        /// <summary>
        /// Gets the async exists based on a predicate.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> selector = null)
        {
            if (selector == null)
            {
                return await _dbSet.AnyAsync();
            }
            else
            {
                return await _dbSet.AnyAsync(selector);
            }
        }

        #endregion

        #region SQL

        /// <summary>
        /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
        /// </summary>
        /// <param name="sql">The raw SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>An <see cref="IQueryable{TEntity}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
        public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters) => _dbSet.FromSqlRaw(sql, parameters);

        #endregion

        #region Get


        /// <summary>
        /// Gets the specified key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Get(params object[] keyValues) => _dbSet.Find(keyValues);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>ValueTask&lt;TEntity&gt;.</returns>
        public virtual ValueTask<TEntity> GetAsync(params object[] keyValues) => _dbSet.FindAsync(keyValues);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>ValueTask&lt;TEntity&gt;.</returns>
        public virtual ValueTask<TEntity> GetAsync(object[] keyValues, CancellationToken cancellationToken) => _dbSet.FindAsync(keyValues, cancellationToken);

        /// <summary>
        /// Gets the specified identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        /// <returns>TModel.</returns>
        public virtual TEntity Get(long identity)
        {
            return _dbSet.Find(identity);
        }

        /// <summary>
        /// Gets the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TModel&gt;.</returns>
        public virtual IQueryable<TEntity> GetAsQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IQueryable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = query;

            return result;
        }

        /// <summary>
        /// Gets as queryable.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public virtual IQueryable<TEntity> GetAsQueryable(
            List<PredicateExpression<TEntity>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IQueryable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filters != null)
            {
                var predicate = PredicateBuilder.New<TEntity>();

                foreach (var filter in filters)
                {
                    if (filters.IndexOf(filter) == 0)
                    {
                        predicate = predicate.Start(filter.Expression);
                    }
                    else
                    {
                        switch (filter.Predicate)
                        {
                            case PredicateOperator.And:
                                predicate = predicate.And(filter.Expression);
                                break;
                            case PredicateOperator.Or:
                                predicate = predicate.Or(filter.Expression);
                                break;
                        }
                    }
                }

                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = query;

            return result;
        }

        /// <summary>
        /// Gets the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TModel&gt;.</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IEnumerable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = query;

            return result;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IEnumerable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = await query.ToListAsync();

            return result;
        }

        /// <summary>
        /// Gets the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        public virtual IEnumerable<TEntity> Get(
            List<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IEnumerable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filters != null)
            {
                var predicate = PredicateBuilder.New<TEntity>();

                foreach (var filter in filters)
                {
                    if (filters.IndexOf(filter) == 0)
                        predicate = predicate.Start(filter);
                    else
                        predicate = predicate.Or(filter);
                }

                query = query.AsExpandable().Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = query;

            return result;
        }

        /// <summary>
        /// Gets as queryable.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public virtual IQueryable<TEntity> GetAsQueryable(
        List<Expression<Func<TEntity, bool>>> filters = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IQueryable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filters != null)
            {
                var predicate = PredicateBuilder.New<TEntity>();

                foreach (var filter in filters)
                {
                    if (filters.IndexOf(filter) == 0)
                    {
                        predicate = predicate.Start(filter);
                    }
                    else
                    {
                        predicate = predicate.Or(filter);
                    }
                }

                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = query;

            return result;
        }

        /// <summary>
        /// Gets the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        public virtual IEnumerable<TEntity> Get(
            List<PredicateExpression<TEntity>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet;
            IEnumerable<TEntity> result = null;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filters != null)
            {
                var predicate = PredicateBuilder.New<TEntity>();

                foreach (var filter in filters)
                {
                    if (filters.IndexOf(filter) == 0)
                    {
                        predicate = predicate.Start(filter.Expression);
                    }
                    else
                    {
                        switch (filter.Predicate)
                        {
                            case PredicateOperator.And:
                                predicate = predicate.And(filter.Expression);
                                break;
                            case PredicateOperator.Or:
                                predicate = predicate.Or(filter.Expression);
                                break;
                        }
                    }
                }

                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
                query = orderBy(query);

            result = query;

            return result;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        /// <summary>
        /// Gets all as queryable.
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        public virtual IQueryable<TEntity> GetAllAsQueryable(out int totalCount, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            totalCount = 0;
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            IQueryable<TEntity> result = null;

            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = 10;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = orderBy(query).Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            else
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = query.Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TModel&gt;.</returns>
        public virtual IEnumerable<TEntity> GetAll(out int totalCount, Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            totalCount = 0;
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            IEnumerable<TEntity> result = null;

            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = 10;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = orderBy(query).Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            else
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = query.Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            return result;
        }

        public virtual async Task<PaginatedResult<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            PaginatedResult<TEntity> result = new PaginatedResult<TEntity>();

            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = -1;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                int total = query.Count(), totalPages = 0;

                if (pageSize > 0)
                {
                    totalPages = total / pageSize;
                    if ((total % pageSize) != 0)
                        totalPages += 1;

                    if (pageNumber <= totalPages)
                    {
                        result.TotalCount = total;
                        result.Page = await orderBy(query).Skip(pageSize * (pageNumber - 1))
                               .Take(pageSize).ToListAsync();

                        return result;
                    }
                }
                else
                {
                    result.TotalCount = total;
                    result.Page = await orderBy(query).ToListAsync();
                }
            }
            else
            {
                int total = query.Count(), totalPages = 0;

                if (pageSize > 0)
                {
                    totalPages = total / pageSize;
                    if ((total % pageSize) != 0)
                        totalPages += 1;
                    if (pageNumber <= totalPages)
                    {
                        result.TotalCount = total;
                        result.Page = await query.Skip(pageSize * (pageNumber - 1))
                               .Take(pageSize).ToListAsync();

                        return result;
                    }
                }
                else
                {
                    result.TotalCount = total;
                    result.Page = await query.ToListAsync();
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        public virtual IEnumerable<TEntity> GetAll(out int totalCount, List<Expression<Func<TEntity, bool>>> filters, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            totalCount = 0;
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            IEnumerable<TEntity> result = null;

            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = 10;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filters != null)
            {
                var predicate = PredicateBuilder.New<TEntity>();

                foreach (var filter in filters)
                {
                    if (filters.IndexOf(filter) == 0)
                    {
                        predicate = predicate.Start(filter);
                    }
                    else
                    {
                        predicate = predicate.Or(filter);
                    }
                }

                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = orderBy(query).Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            else
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = query.Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        public virtual IEnumerable<TEntity> GetAll(out int totalCount, List<PredicateExpression<TEntity>> filters, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false)
        {
            totalCount = 0;
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            IEnumerable<TEntity> result = null;

            if (pageNumber <= 0)
                pageNumber = 1;
            if (pageSize <= 0)
                pageSize = 10;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (filters != null)
            {
                var predicate = PredicateBuilder.New<TEntity>();

                foreach (var filter in filters)
                {
                    if (filters.IndexOf(filter) == 0)
                    {
                        predicate = predicate.Start(filter.Expression);
                    }
                    else
                    {
                        switch (filter.Predicate)
                        {
                            case PredicateOperator.And:
                                predicate = predicate.And(filter.Expression);
                                break;
                            case PredicateOperator.Or:
                                predicate = predicate.Or(filter.Expression);
                                break;
                        }
                    }
                }

                query = query.Where(predicate);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }

            if (orderBy != null)
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = orderBy(query).Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            else
            {
                int total = query.Count(), totalPages = 0;
                totalCount = total;
                totalPages = total / pageSize;
                if ((total % pageSize) != 0)
                    totalPages += 1;
                if (pageNumber <= totalPages)
                {
                    result = query.Skip(pageSize * (pageNumber - 1))
                           .Take(pageSize);

                    return result;
                }
            }
            return result;
        }

        #endregion

        #region Bulk

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void BulkInsert(IList<TEntity> entity)
        {
            try
            {
                _dbContext.BulkInsert(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the insert asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        public virtual Task BulkInsertAsync(IList<TEntity> entity)
        {
            try
            {
                return _dbContext.BulkInsertAsync(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void BulkUpdate(IList<TEntity> entity)
        {
            try
            {
                _dbContext.BulkUpdate(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the update asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        public virtual Task BulkUpdateAsync(IList<TEntity> entity)
        {
            try
            {
                return _dbContext.BulkUpdateAsync(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the insert or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void BulkInsertOrUpdate(IList<TEntity> entity)
        {
            try
            {
                _dbContext.BulkInsertOrUpdate(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the insert or update asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        public virtual Task BulkInsertOrUpdateAsync(IList<TEntity> entity)
        {
            try
            {
                return _dbContext.BulkInsertOrUpdateAsync(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the delete.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void BulkDelete(IList<TEntity> entity)
        {
            try
            {
                _dbContext.BulkDelete(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the delete asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        public virtual Task BulkDeleteAsync(IList<TEntity> entity)
        {
            try
            {
                return _dbContext.BulkDeleteAsync(entity);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the update with query.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <returns>System.Int32.</returns>
        public virtual int BulkUpdateWithQuery(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> query)
        {
            try
            {
                return _dbSet.Where(where).BatchUpdate(query);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the update with query asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public virtual Task<int> BulkUpdateWithQueryAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> query)
        {
            try
            {
                return _dbSet.Where(where).BatchUpdateAsync(query);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the delete with query.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>System.Int32.</returns>
        public virtual int BulkDeleteWithQuery(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                return _dbSet.Where(where).BatchDelete();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Bulks the delete with query asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        public virtual Task<int> BulkDeleteWithQueryAsync(Expression<Func<TEntity, bool>> where)
        {
            try
            {
                return _dbSet.Where(where).BatchDeleteAsync();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #endregion

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _dbContext.Set<TEntity>().SingleOrDefaultAsync(match);
        }
        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
