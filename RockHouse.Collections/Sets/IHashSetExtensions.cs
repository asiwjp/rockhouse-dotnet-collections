using System.Collections.Generic;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// Provides extension methods for IHashSet.
    /// </summary>
    public static class IHashSetExtensions
    {
        /// <summary>
        /// Copy all elements into the collection.
        /// </summary>
        /// <param name="set">Collection to be extended.</param>
        /// <param name="src">The enumerable from which to copy.</param>
        /// <returns>Returns true if Set has changed, false otherwise.</returns>
        public static bool AddAll<T>(this IHashSet<T> set, IEnumerable<T> src)
        {
            bool added = false;
            foreach (var item in src)
            {
                added |= set.Add(item);
            }
            return added;
        }
    }
}
