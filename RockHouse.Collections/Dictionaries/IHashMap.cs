using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries
{
    /// <summary>
    /// IHashMap is an imitation of the Java language's Map interface.
    /// </summary>
    /// <typeparam name="K">The type of keys.</typeparam>
    /// <typeparam name="V">The type of values.</typeparam>
    public interface IHashMap<K, V> : IDictionary<K, V>, IContainer where K : notnull
    {
    }
}
