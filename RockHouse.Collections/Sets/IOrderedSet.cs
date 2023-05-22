namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// An interface representing an ordered set.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public interface IOrderedSet<T> : IHashSet<T>
    {
        /// <summary>
        /// The first element.
        /// </summary>
        T First { get; }

        /// <summary>
        /// The last element.
        /// </summary>
        T Last { get; }
    }
}
