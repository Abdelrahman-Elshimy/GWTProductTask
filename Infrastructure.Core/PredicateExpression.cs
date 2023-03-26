using System;
using System.Linq.Expressions;
namespace Infrastructure.Core
{
    /// <summary>
    /// Class PredicateExpression.
    /// </summary>
    /// <typeparam name="TEntity">The type of the t entity.</typeparam>
    public class PredicateExpression<TEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PredicateExpression{TEntity}"/> class.
        /// </summary>
        public PredicateExpression()
        {
            Predicate = PredicateOperator.And;
        }

        /// <summary>
        /// Gets or sets the predicate.
        /// </summary>
        /// <value>The predicate.</value>
        public PredicateOperator Predicate { get; set;}
        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>The expression.</value>
        public Expression<Func<TEntity, bool>> Expression { get; set; }
    }
}
