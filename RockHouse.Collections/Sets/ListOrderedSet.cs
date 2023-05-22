using RockHouse.Collections.Sets.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// ListOrderedSet is an ordered Set that allows you to retrieve items in the order in which they were registered.
    /// The order is managed by List.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    [JsonConverter(typeof(SetJsonConverterFactory))]
    public class ListOrderedSet<T> : AbstractOrderedSet<T>
    {
        private readonly Dictionary<ValueTuple<T>, int> _dic;
        private readonly List<T> _list;
        public ListOrderedSet() : this(0)
        {
        }

        public ListOrderedSet(int capacity)
        {
            this._dic = new Dictionary<ValueTuple<T>, int>(capacity);
            this._list = new List<T>(capacity);
        }

        public ListOrderedSet(IEnumerable<T> other) : this(0)
        {
            this.AddAll(other);
        }

        #region ICollection
        public override int Count => this._dic.Count;

        public override bool IsReadOnly => false;

        public override bool Add(T item)
        {
            var keyPacket = new ValueTuple<T>(item);
            if (this._dic.ContainsKey(keyPacket))
            {
                return false;
            }

            var i = this._list.Count;
            this._list.Add(item);
            this._dic.Add(keyPacket, i);
            return true;
        }

        public override void Clear()
        {
            this._list.Clear();
            this._dic.Clear();
        }

        public override bool Contains(T item)
        {
            return this._dic.ContainsKey(new ValueTuple<T>(item));
        }

        public override bool Remove(T item)
        {
            var querySlot = new ValueTuple<T>(item);
            if (!this._dic.TryGetValue(querySlot, out int index))
            {
                return false;
            }

            var lastOfBefore = this._list.Count - 1;
            this._list.RemoveAt(index);
            this._dic.Remove(querySlot);

            if (index < lastOfBefore)
            {
                this.ReIndex(index);
            }

            return true;
        }

        private void ReIndex(int gapStart)
        {
            var keyPacket = new ValueTuple<T>();
            for (var i = gapStart; i < _list.Count; ++i)
            {
                keyPacket.Item1 = _list[i];
                this._dic[keyPacket] = i;
            }
        }
        #endregion

        #region
        public override IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this._list)
            {
                yield return item;
            }
        }
        #endregion

        #region IOrderedSet
        public override T First
        {
            get
            {
                this.CheckEmpty();
                return this._list[0];
            }
        }

        public override T Last
        {
            get
            {
                this.CheckEmpty();
                return this._list[this._list.Count - 1];
            }
        }
        #endregion
    }
}
