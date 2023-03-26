using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    /// <summary>
    /// Interface IUnitOfWork
    /// </summary>
    /// <typeparam name="TContext">The type of the t context.</typeparam>
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        TContext DbContext { get; }
    }

    public interface IUnitOfWork
    {
        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <returns>IRepository&lt;TEntity&gt;.</returns>
        IRepository<TEntity> GetRepository<TEntity>()
                    where TEntity : class;
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>DbContextTransaction.</returns>
        IDbContextTransaction BeginTransaction();
        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>System.Int32.</returns>
        int SaveChanges();
        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task&lt;System.Int32&gt;.</returns>
        Task<int> SaveChangesAsync();
    }
}
