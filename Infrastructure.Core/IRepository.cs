using Infrastracture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Core
{

    /// <summary>
    /// Interface IRepository
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public interface IRepository<TEntity>
    where TEntity : class
    {
        #region Create

        /// <summary>
        /// Creates the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>TEntity.</returns>
        TEntity Create(TEntity data);
        /// <summary>
        /// Creates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Create(params TEntity[] entities);
        /// <summary>
        /// Creates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Create(IEnumerable<TEntity> entities);

        #endregion

        #region Update

        /// <summary>
        /// Updates the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        void Update(TEntity data);
        void Update(TEntity entity, object entityID);
        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Update(params TEntity[] entities);
        /// <summary>
        /// Updates the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Update(IEnumerable<TEntity> entities);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the specified identity.
        /// </summary>
        /// <param name="identity">The identity.</param>
        void Delete(object identity);
        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(params TEntity[] entities);
        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<TEntity> entities);

        #endregion

        #region Count

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        int Count(Expression<Func<TEntity, bool>> predicate = null);
        /// <summary>
        /// Counts the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        /// <summary>
        /// Longs the count.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int64.</returns>
        long LongCount(Expression<Func<TEntity, bool>> predicate = null);
        /// <summary>
        /// Longs the count asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>Task&lt;System.Int64&gt;.</returns>
        Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null);

        #endregion

        #region Aggregate

        /// <summary>
        /// Determines the maximum of the parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>T.</returns>
        T Max<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);
        /// <summary>
        /// Maximums the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> MaxAsync<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);
        /// <summary>
        /// Determines the minimum of the parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>T.</returns>
        T Min<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);
        /// <summary>
        /// Minimums the asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>Task&lt;T&gt;.</returns>
        Task<T> MinAsync<T>(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, T>> selector = null);
        /// <summary>
        /// Averages the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>System.Decimal.</returns>
        decimal Average(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);
        /// <summary>
        /// Averages the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>Task&lt;System.Decimal&gt;.</returns>
        Task<decimal> AverageAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);
        /// <summary>
        /// Sums the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>System.Decimal.</returns>
        decimal Sum(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);
        /// <summary>
        /// Sums the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="selector">The selector.</param>
        /// <returns>Task&lt;System.Decimal&gt;.</returns>
        Task<decimal> SumAsync(Expression<Func<TEntity, bool>> predicate = null, Expression<Func<TEntity, decimal>> selector = null);
        /// <summary>
        /// Existses the specified selector.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Exists(Expression<Func<TEntity, bool>> selector = null);
        /// <summary>
        /// Existses the asynchronous.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> selector = null);

        #endregion

        #region

        /// <summary>
        /// Froms the SQL.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);

        #endregion

        #region Get

        /// <summary>
        /// Gets the specified key values.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>TEntity.</returns>
        TEntity Get(params object[] keyValues);
        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <returns>ValueTask&lt;TEntity&gt;.</returns>
        ValueTask<TEntity> GetAsync(params object[] keyValues);
        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="keyValues">The key values.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>ValueTask&lt;TEntity&gt;.</returns>
        ValueTask<TEntity> GetAsync(object[] keyValues, CancellationToken cancellationToken);
        /// <summary>
        /// Gets the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TModel&gt;.</returns>
        IEnumerable<TEntity> Get(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        /// <summary>
        /// Gets as queryable.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> GetAsQueryable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        /// <summary>
        /// Gets as queryable.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> GetAsQueryable(
            List<PredicateExpression<TEntity>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        /// <summary>
        /// Gets the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> Get(
            List<Expression<Func<TEntity, bool>>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        /// <summary>
        /// Gets as queryable.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> GetAsQueryable(
        List<Expression<Func<TEntity, bool>>> filters = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        /// <summary>
        /// Gets the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <param name="disableTracking">if set to <c>true</c> [disable tracking].</param>
        /// <param name="ignoreQueryFilters">if set to <c>true</c> [ignore query filters].</param>
        /// <returns>IEnumerable&lt;TEntity&gt;.</returns>
        IEnumerable<TEntity> Get(
            List<PredicateExpression<TEntity>> filters = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool disableTracking = true, bool ignoreQueryFilters = false);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> GetAll();
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
        IQueryable<TEntity> GetAllAsQueryable(out int totalCount, Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false);
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
        IEnumerable<TEntity> GetAll(out int totalCount, Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false);
        Task<PaginatedResult<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false);
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
        IEnumerable<TEntity> GetAll(out int totalCount, List<Expression<Func<TEntity, bool>>> filters, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
           string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false);
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
        IEnumerable<TEntity> GetAll(out int totalCount, List<PredicateExpression<TEntity>> filters, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, int pageNumber = 1, int pageSize = 10, bool disableTracking = true, bool ignoreQueryFilters = false);
        #endregion

        #region Batch

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <param name="data">The data.</param>
        void BulkInsert(IList<TEntity> data);
        /// <summary>
        /// Bulks the insert asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task BulkInsertAsync(IList<TEntity> entity);
        /// <summary>
        /// Bulks the update.
        /// </summary>
        /// <param name="data">The data.</param>
        void BulkUpdate(IList<TEntity> data);
        /// <summary>
        /// Bulks the update asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task BulkUpdateAsync(IList<TEntity> entity);
        /// <summary>
        /// Bulks the insert or update.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void BulkInsertOrUpdate(IList<TEntity> entity);
        /// <summary>
        /// Bulks the insert or update asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task BulkInsertOrUpdateAsync(IList<TEntity> entity);
        /// <summary>
        /// Bulks the delete.
        /// </summary>
        /// <param name="data">The data.</param>
        void BulkDelete(IList<TEntity> data);
        /// <summary>
        /// Bulks the delete asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task BulkDeleteAsync(IList<TEntity> entity);
        /// <summary>
        /// Bulks the update with query.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <returns>System.Int32.</returns>
        int BulkUpdateWithQuery(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> query);
        /// <summary>
        /// Bulks the update with query asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <param name="query">The query.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> BulkUpdateWithQueryAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> query);
        /// <summary>
        /// Bulks the delete with query.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>System.Int32.</returns>
        int BulkDeleteWithQuery(Expression<Func<TEntity, bool>> where);
        /// <summary>
        /// Bulks the delete with query asynchronous.
        /// </summary>
        /// <param name="where">The where.</param>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> BulkDeleteWithQueryAsync(Expression<Func<TEntity, bool>> where);

        #endregion

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> match);
        Task<int> CompleteAsync();

    }
}
