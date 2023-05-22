namespace RockHouse.Collections.Slots
{
    public abstract class AbstractSlot : IContainer, ISlot
    {
        public abstract int Count { get; }

        public bool IsEmpty => this.Count == 0;

        #region ITuple
        public abstract object this[int index] { get; }

        public abstract int Length { get; }
        #endregion

        #region ISlot
        public abstract bool IsFree(int index);

        public abstract void Set(int index, object value);

        public abstract int CompareTo(ISlot? obj);
        #endregion

    }
}
