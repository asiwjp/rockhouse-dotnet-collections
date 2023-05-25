using System;

namespace RockHouse.Collections
{
    /// <summary>
    /// An abstract implementation of all collecton.
    /// </summary>
    public abstract class AbstractCollection : IContainer
    {
        /// <summary>
        /// Determines if the collection is empty or not and throws an exception if empty.
        /// </summary>
        /// <exception cref="InvalidOperationException">if the collection is empty.</exception>
        protected void CheckEmpty()
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("Collection is empty.");
            }
        }

        #region IContainer
        /// <inheritdoc/>
        public abstract bool IsEmpty { get; }
        #endregion
    }
}
