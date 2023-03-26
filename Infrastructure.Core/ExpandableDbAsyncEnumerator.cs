using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    /// <summary> Class for async-await style list enumeration support (e.g. .ToListAsync())</summary>
    public sealed class ExpandableDbAsyncEnumerator<T> : IDisposable,IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        /// <summary> Class for async-await style list enumeration support (e.g. .ToListAsync())</summary>
        public ExpandableDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        /// <summary> Dispose, .NET using-pattern </summary>
        public void Dispose()
        {
            _inner.Dispose();
        }

        /// <summary> Enumerator-pattern: MoveNext </summary>
        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        /// <summary> Enumerator-pattern: Current item </summary>
        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return new ValueTask();
        }

        /// <summary> Enumerator-pattern: Current item </summary>
        public T Current => _inner.Current;

    }
}
