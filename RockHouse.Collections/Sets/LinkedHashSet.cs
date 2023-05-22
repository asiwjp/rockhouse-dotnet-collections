using RockHouse.Collections.Sets.Json.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// LinkedHashSet is a class that has almost the same functionality as LinkedOrderedSet.
    /// This class is intended for programmers familiar with the Java language.
    /// </summary>
    /// <typeparam name="K">The type of elements.</typeparam>
    [JsonConverter(typeof(SetJsonConverterFactory))]
    public class LinkedHashSet<T> : LinkedOrderedSet<T>
    {
        public LinkedHashSet() { }

        public LinkedHashSet(int capacity) : base(capacity) { }

        public LinkedHashSet(IEnumerable<T> src) : base(src) { }
    }
}
