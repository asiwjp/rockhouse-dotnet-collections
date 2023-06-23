using RockHouse.Collections.Sets.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// LinkedHashSet is a class that has almost the same functionality as LinkedOrderedSet.
    /// This class is intended for programmers familiar with the Java language.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    [JsonConverter(typeof(SetJsonConverterFactory))]
    public class LinkedHashSet<T> : LinkedOrderedSet<T>
    {
        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public LinkedHashSet() : this(0, null) { }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public LinkedHashSet(int capacity) : base(capacity, null) { }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public LinkedHashSet(IEnumerable<T> src) : base(src, null) { }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LinkedHashSet(IEqualityComparer<T>? comparer) : base(0, comparer) { }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LinkedHashSet(int capacity, IEqualityComparer<T>? comparer) : base(capacity, comparer) { }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LinkedHashSet(IEnumerable<T> src, IEqualityComparer<T>? comparer) : base(src, comparer) { }
    }
}
