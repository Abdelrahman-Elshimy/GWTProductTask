using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    /// <summary>
    /// Class UnitOfWork.
    /// </summary>
    /// <typeparam name="TContext">The type of the t context.</typeparam>
    /// <seealso cref="Infrastructure.Core.IUnitOfWork{TContext}" />
    public class UnitOfWork<TContext> : IDisposable, IUnitOfWork<TContext> where TContext : DbContext
    {
        #region Fields

        /// <summary>
        /// The context
        /// </summary>
        private readonly TContext _context;
        /// <summary>
        /// The disposed
        /// </summary>
        private bool disposed = false;
        /// <summary>
        /// The repositories cache
        /// </summary>
        private Dictionary<Type, object> repositories;
        /// <summary>
        /// The dp repositories
        /// </summary>
        private Dictionary<Type, object> dpRepositories;
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration _configuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(IConfiguration configuration, TContext context)
        {
            _configuration = configuration;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        public TContext DbContext
        {
            get
            {
                return this._context;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <returns>IRepository&lt;TEntity, TModel&gt;.</returns>
        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new GenericRepository<TEntity>(_context);
            }

            return (IRepository<TEntity>)repositories[type];
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="callBack">The call back.</param>
        public void ExecuteStoredProcedure(string name, List<SqlParameter> parameters, Action<SqlCommand> callBack)
        {
            var connection = _context.Database.GetDbConnection();
            if (connection is SqlConnection)
            {
                using (SqlConnection sqlConnection = (SqlConnection)connection)
                {
                    using (SqlCommand cmd = new SqlCommand(name, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter.Name, parameter.Type).Value = parameter.Value;
                        }

                        connection.Open();
                        callBack.Invoke(cmd);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public List<T> ExecuteStoredProcedure<T>(string name, List<SqlParameter> parameters)
        {
            List<T> result = new List<T>();

            var connection = _context.Database.GetDbConnection();
            if (connection is SqlConnection)
            {
                using (SqlConnection sqlConnection = (SqlConnection)connection)
                {
                    using (SqlCommand cmd = new SqlCommand(name, sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter.Name, parameter.Type).Value = parameter.Value;
                        }

                        connection.Open();

                        DataSet ds = new DataSet();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds);
                        }

                        if (ds.Tables != null && ds.Tables.Count > 0)
                            result = ds.Tables[0].ToGeneric<T>();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>DbContextTransaction.</returns>
        public IDbContextTransaction BeginTransaction()
        {
            IDbContextTransaction dbContextTransaction = null;
            if (_context != null)
                dbContextTransaction = _context.Database.BeginTransaction();
            return dbContextTransaction;
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int SaveChanges()
        {
            if (_context != null)
                return _context.SaveChanges();
            return -1;
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public Task<int> SaveChangesAsync()
        {
            if (_context != null)
                return _context.SaveChangesAsync();
            return Task.FromResult(-1);
        }

        #region Dispose

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">The disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // clear repositories
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }

                    // dispose the db context.
                    _context.Dispose();
                }
            }

            disposed = true;
        }

        #endregion

        #endregion
    }
}
