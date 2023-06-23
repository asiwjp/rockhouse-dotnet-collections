using System.Collections.Generic;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// An abstract implementation of a IOrderedSet. 
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public abstract class AbstractOrderedSet<T> : AbstractSet<T>, IOrderedSet<T>
    {
        /// <summary>
        /// Constructs an instance with the specified arguments.
        /// </summary>
        /// <param name="comparer">A comparer that compares elements.</param>
        protected AbstractOrderedSet(IEqualityComparer<T>? comparer) : base(comparer) { }

        #region IOrderedSet
        /// <inheritdoc/>
        public abstract T First { get; }

        /// <inheritdoc/>
        public abstract T Last { get; }
        #endregion
    }
}
