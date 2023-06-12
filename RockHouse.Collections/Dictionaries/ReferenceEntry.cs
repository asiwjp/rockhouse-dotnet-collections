using System.Collections.Generic;

namespace RockHouse.Collections.Dictionaries
{
    internal readonly struct ReferenceEntry<K, V>
    {
        public readonly AbstractReferenceHolder Key;
        public readonly AbstractReferenceHolder Value;

        public ReferenceEntry(ReferenceStrength keyStrength, K key, ReferenceStrength valueStrength, V value)
        {
            Key = keyStrength.Hold(key);
            Value = valueStrength.Hold(value);
        }

        public KeyValuePair<K, V>? GetKeyValue()
        {
            if (!Key.TryGet(out var key))
            {
                return null;
            }

            if (!Value.TryGet(out var value))
            {
                return null;
            }

            return new KeyValuePair<K, V>((K)key, (V)value);
        }
    }
}
