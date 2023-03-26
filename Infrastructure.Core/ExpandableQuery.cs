using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Infrastructure.Core
{
    /// <summary>
    /// An IQueryable wrapper that allows us to visit the query's expression tree just before LINQ to SQL gets to it.
    /// This is based on the excellent work of Tomas Petricek: http://tomasp.net/blog/linq-expand.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpandableQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable, IAsyncEnumerable<T>
    {
        readonly ExpandableQueryProvider<T> _provider;
        readonly IQueryable<T> _inner;

        internal IQueryable<T> InnerQuery => _inner; // Original query, that we're wrapping

        internal ExpandableQuery(IQueryable<T> inner, Func<Expression, Expression> queryOptimizer)
        {
            _inner = inner;
            _provider = new ExpandableQueryProvider<T>(this, queryOptimizer);
        }

        Expression IQueryable.Expression => _inner.Expression;

        Type IQueryable.ElementType => typeof(T);

        IQueryProvider IQueryable.Provider => _provider;

        /// <summary> IQueryable enumeration </summary>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        /// <summary>
        /// IQueryable string presentation.
        /// </summary>
        public override string ToString() { return _inner.ToString(); }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

    class ExpandableQueryProvider<T> : IQueryProvider, IAsyncQueryProvider
    {
        readonly ExpandableQuery<T> _query;
        readonly Func<Expression, Expression> _queryOptimizer;

        internal ExpandableQueryProvider(ExpandableQuery<T> query, Func<Expression, Expression> queryOptimizer)
        {
            _query = query;
            _queryOptimizer = queryOptimizer;
        }

        // The following four methods first call ExpressionExpander to visit the expression tree, then call
        // upon the inner query to do the remaining work.
        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            var expanded = expression.Expand();
            var optimized = _queryOptimizer(expanded);
            return _query.InnerQuery.Provider.CreateQuery<TElement>(optimized).AsExpandable();
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return _query.InnerQuery.Provider.CreateQuery(expression.Expand());
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            var expanded = expression.Expand();
            var optimized = _queryOptimizer(expanded);
            return _query.InnerQuery.Provider.Execute<TResult>(optimized);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            var expanded = expression.Expand();
            var optimized = _queryOptimizer(expanded);
            return _query.InnerQuery.Provider.Execute(optimized);
        }
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            var asyncProvider = _query.InnerQuery.Provider as IAsyncQueryProvider;
            return (IAsyncEnumerable<TResult>)asyncProvider.ExecuteAsync<TResult>(expression.Expand());
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var asyncProvider = _query.InnerQuery.Provider as IAsyncQueryProvider;

            var expanded = expression.Expand();
            var optimized = _queryOptimizer(expanded);
            if (asyncProvider != null)
            {
                return asyncProvider.ExecuteAsync<TResult>(optimized, cancellationToken);
            }

            return _query.InnerQuery.Provider.Execute<TResult>(optimized);
        }
    }
}
