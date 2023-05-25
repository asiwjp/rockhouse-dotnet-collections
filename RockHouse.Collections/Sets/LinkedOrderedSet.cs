using RockHouse.Collections.Sets.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// LinkedOrderedSet is an ordered Set that allows you to retrieve items in the order in which they were registered.
    /// The order is managed by LinkedList.
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    [JsonConverter(typeof(SetJsonConverterFactory))]
    public class LinkedOrderedSet<T> : AbstractOrderedSet<T>
    {
        private readonly Dictionary<ValueTuple<T>, LinkedListNode<T>> _dic;
        private readonly LinkedList<T> _list;

        public LinkedOrderedSet() : this(0)
        {
        }

        public LinkedOrderedSet(int capacity)
        {
            this._dic = new Dictionary<ValueTuple<T>, LinkedListNode<T>>(capacity);
            this._list = new LinkedList<T>();
        }

        public LinkedOrderedSet(IEnumerable<T> src) : this(0)
        {
            this.AddAll(src);
        }

        #region ICollection
        /// <inheritdoc/>
        public override int Count => this._dic.Count;

        /// <inheritdoc/>
        public override bool IsReadOnly => false;

        /// <inheritdoc/>
        public override bool Add(T item)
        {
            var keyPacket = new ValueTuple<T>(item);
            if (this._dic.ContainsKey(keyPacket))
            {
                return false;
            }

            var node = this._list.AddLast(item);
            this._dic.Add(keyPacket, node);
            return true;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            this._list.Clear();
            this._dic.Clear();
        }

        /// <inheritdoc/>
        public override bool Contains(T item)
        {
            return this._dic.ContainsKey(new ValueTuple<T>(item));
        }

        /// <inheritdoc/>
        public override bool Remove(T item)
        {
            var keyPacket = new ValueTuple<T>(item);
            if (!this._dic.TryGetValue(keyPacket, out LinkedListNode<T>? node))
            {
                return false;
            }
            this._list.Remove(node);
            this._dic.Remove(keyPacket);

            return true;
        }
        #endregion

        #region IEnumerable
        /// <inheritdoc/>
        public override IEnumerator<T> GetEnumerator()
        {
            foreach (var item in this._list)
            {
                yield return item;
            }
        }
        #endregion

        #region IOrderedSet
        /// <inheritdoc/>
        public override T First
        {
            get
            {
                this.CheckEmpty();
                return this._list.First.Value;
            }
        }

        /// <inheritdoc/>
        public override T Last
        {
            get
            {
                this.CheckEmpty();
                return this._list.Last.Value;
            }
        }
        #endregion
    }
}
