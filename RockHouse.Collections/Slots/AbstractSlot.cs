namespace RockHouse.Collections.Slots
{
    /// <summary>
    /// An abstract implementation of a ITuple/ISlot. 
    /// </summary>
    public abstract class AbstractSlot : IContainer, ISlot
    {
        /// <inheritdoc/>
        public abstract int Count { get; }

        /// <inheritdoc/>
        public bool IsEmpty => this.Count == 0;

        #region ITuple
        /// <inheritdoc/>
        public abstract object this[int index] { get; }

        /// <inheritdoc/>
        public abstract int Length { get; }
        #endregion

        #region ISlot
        /// <inheritdoc/>
        public abstract bool IsFree(int index);

        /// <inheritdoc/>
        public abstract void Set(int index, object value);

        /// <inheritdoc/>
        public abstract int CompareTo(ISlot? obj);
        #endregion

    }
}
