using System;
using System.Collections.Generic;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// This is a read-only Set.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public class ReadOnlySet<T> : AbstractSet<T>
    {
        private readonly ISet<T> _set;

        /// <summary>
        /// Constructs an instance.
        /// </summary>
        public ReadOnlySet() : base(null)
        {
            this._set = new HashSet<T>();
        }

        /// <summary>
        /// Constructs an instance with the set.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public ReadOnlySet(ISet<T> src) : base(null)
        {
            this._set = src;
        }

        #region ICollection
        /// <inheritdoc/>
        public override int Count => this._set.Count;

        /// <inheritdoc/>
        public override bool IsReadOnly => true;

        /// <inheritdoc/>
        public override bool Add(T item)
        {
            throw new NotSupportedException("This collections is read-only.");
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            throw new NotSupportedException("This collections is read-only.");
        }

        /// <inheritdoc/>
        public override bool Contains(T item)
        {
            return this._set.Contains(item);
        }

        /// <inheritdoc/>
        public override bool Remove(T item)
        {
            throw new NotSupportedException("This collections is read-only.");
        }
        #endregion

        #region IEnumerable
        /// <inheritdoc/>
        public override IEnumerator<T> GetEnumerator()
        {
            return this._set.GetEnumerator();
        }
        #endregion
    }
}
