namespace RockHouse.Collections
{
    /// <summary>
    /// This interface represents a container that can store and contain elements.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Determine if this collection is empty or not.
        /// </summary>
        /// <returns>true if this collection is empty, otherwise false.</returns>
        bool IsEmpty { get; }
    }
}
