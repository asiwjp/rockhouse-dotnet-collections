﻿using RockHouse.Collections.Sets;
using System.Collections.Generic;
using System.Text.Json;

namespace RockHouse.Collections.Tests.Sets
{
    public class ListOrderedSetTest : AbstractOrderedSetTestBase
    {
        public override AbstractOrderedSet<T> NewInstance<T>()
        {
            return new ListOrderedSet<T>();
        }

        public override AbstractOrderedSet<T> NewInstance<T>(int capacity)
        {
            return new ListOrderedSet<T>(capacity);
        }

        public override AbstractOrderedSet<string> NewInstance(IEnumerable<string> src)
        {
            return new ListOrderedSet<string>(src);
        }

        public override AbstractOrderedSet<string> NewInstance(ISet<string> src)
        {
            return new ListOrderedSet<string>(src);
        }

        public override AbstractOrderedSet<T> Deserialize_BySystemTextJson<T>(string json)
        {
            return JsonSerializer.Deserialize<ListOrderedSet<T>>(json);
        }

        public override string Serialize_BySystemTextJson<T>(AbstractOrderedSet<T> src)
        {
            return JsonSerializer.Serialize(src as ListOrderedSet<T>);
        }
    }
}
