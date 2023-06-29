using RockHouse.Collections.Sets;
using System.Collections.Generic;
using System.Text.Json;

namespace Tests.Sets
{
    public class LinkedHashSetTest : AbstractOrderedSetTestBase
    {
        public override AbstractOrderedSet<T> NewInstance<T>()
        {
            return new LinkedHashSet<T>();
        }

        public override AbstractOrderedSet<T> NewInstance<T>(int capacity)
        {
            return new LinkedHashSet<T>(capacity);
        }

        public override AbstractOrderedSet<string> NewInstance(IEnumerable<string> src)
        {
            return new LinkedHashSet<string>(src);
        }

        public override AbstractOrderedSet<string> NewInstance(ISet<string> src)
        {
            return new LinkedHashSet<string>(src);
        }

        public override AbstractOrderedSet<T> NewInstance<T>(IEqualityComparer<T>? comparer)
        {
            return new LinkedHashSet<T>(comparer);
        }

        public override AbstractOrderedSet<T> NewInstance<T>(int capacity, IEqualityComparer<T>? comparer)
        {
            return new LinkedHashSet<T>(capacity, comparer);
        }

        public override AbstractOrderedSet<T> NewInstance<T>(IEnumerable<T> src, IEqualityComparer<T>? comparer)
        {
            return new LinkedHashSet<T>(src, comparer);
        }

        public override AbstractOrderedSet<T> Deserialize_BySystemTextJson<T>(string json)
        {
            return JsonSerializer.Deserialize<LinkedHashSet<T>>(json);
        }

        public override string Serialize_BySystemTextJson<T>(AbstractOrderedSet<T> src)
        {
            return JsonSerializer.Serialize(src as LinkedHashSet<T>);
        }
    }
}
