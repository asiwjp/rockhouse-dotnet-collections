using System.Collections;
using System.Collections.Generic;

namespace RockHouse.Collections
{
    /// <summary>
    /// Provides utility methods for collections.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Get the number of elements in the collection.
        /// This method is null-safe.
        /// </summary>
        /// <param name="collection">An instance of a collection. Or null.</param>
        /// <returns>The number of elements in the collection, 0 if null.</returns>
        public static int Count(ICollection? collection)
        {
            return collection?.Count ?? 0;
        }

        /// <summary>
        /// Determines if the collection is empty or not.
        /// This method is null-safe.
        /// </summary>
        /// <param name="collection">An instance of a collection. Or null.</param>
        /// <returns>True if the collection is null or the number of elements in the collection is zero. otherwise False.</returns>
        public static bool IsEmpty(ICollection? collection)
        {
            return Count(collection) == 0;
        }

        /// <summary>
        /// Determines if the collection is empty or not.
        /// This method is null-safe.
        /// </summary>
        /// <param name="collection">An instance of a collection. Or null.</param>
        /// <returns>True if the number of elements in the collection is non-zero, false if the number of elements is zero or the collection is null.</returns>
        public static bool IsNotEmpty(ICollection? collection)
        {
            return Count(collection) != 0;
        }
    }
}
