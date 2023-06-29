using RockHouse.Collections.Sets;
using System.Collections.Generic;
using System.Text.Json;

namespace Tests.Sets
{
    public class LinkedOrderedSetTest : AbstractOrderedSetTestBase
    {
        public override AbstractOrderedSet<T> NewInstance<T>()
        {
            return new LinkedOrderedSet<T>();
        }

        public override AbstractOrderedSet<T> NewInstance<T>(int capacity)
        {
            return new LinkedOrderedSet<T>(capacity);
        }

        public override AbstractOrderedSet<string> NewInstance(IEnumerable<string> src)
        {
            return new LinkedOrderedSet<string>(src);
        }

        public override AbstractOrderedSet<string> NewInstance(ISet<string> src)
        {
            return new LinkedOrderedSet<string>(src);
        }

        public override AbstractOrderedSet<T> NewInstance<T>(IEqualityComparer<T>? comparer)
        {
            return new LinkedOrderedSet<T>(comparer);
        }

        public override AbstractOrderedSet<T> NewInstance<T>(int capacity, IEqualityComparer<T>? comparer)
        {
            return new LinkedOrderedSet<T>(capacity, comparer);
        }

        public override AbstractOrderedSet<T> NewInstance<T>(IEnumerable<T> src, IEqualityComparer<T>? comparer)
        {
            return new LinkedOrderedSet<T>(src, comparer);
        }

        public override AbstractOrderedSet<T> Deserialize_BySystemTextJson<T>(string json)
        {
            return JsonSerializer.Deserialize<LinkedOrderedSet<T>>(json);
        }

        public override string Serialize_BySystemTextJson<T>(AbstractOrderedSet<T> src)
        {
            return JsonSerializer.Serialize(src as LinkedOrderedSet<T>);
        }
    }
}
