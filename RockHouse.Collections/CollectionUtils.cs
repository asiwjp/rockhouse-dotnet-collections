using System.Collections;

namespace RockHouse.Collections
{
    /// <summary>
    /// Provides utility methods for collections.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        /// Get the number of elements in the collection.
        /// It differs from Linq's Count() in that it tolerates null.
        /// </summary>
        /// <param name="collection">An instance of a collection. Or null.</param>
        /// <returns>The number of elements in the collection, 0 if null.</returns>
        public static int Count(ICollection? collection)
        {
            return EnumerableUtils.Count(collection);
        }

        /// <summary>
        /// Determines if the Collection is empty or not.
        /// It differs from Linq's Any() in that it tolerates null.
        /// </summary>
        /// <param name="collection">An instance of a collection. Or null.</param>
        /// <returns>True if the collection is null or the number of elements in the collection is zero. otherwise False.</returns>
        public static bool IsEmpty(ICollection? collection)
        {
            return EnumerableUtils.IsEmpty(collection);
        }

        /// <summary>
        /// Determines if the Collection is empty or not.
        /// </summary>
        /// <param name="collection">An instance of a collection. Or null.</param>
        /// <returns>True if the number of elements in the collection is non-zero, false if the number of elements is zero or the collection is null.</returns>
        public static bool IsNotEmpty(ICollection? collection)
        {
            return EnumerableUtils.IsNotEmpty(collection);
        }
    }
}
