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

        /// <summary>
        /// Constructs an empty instance.
        /// </summary>
        public LinkedOrderedSet() : this(0, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        public LinkedOrderedSet(int capacity) : this(capacity, null)
        {
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        public LinkedOrderedSet(IEnumerable<T> src) : this(src, null)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LinkedOrderedSet(IEqualityComparer<T>? comparer) : this(0, comparer)
        {
        }

        /// <summary>
        /// Constructs an empty instance with the specified arguments.
        /// </summary>
        /// <param name="capacity">Initial capacity of the collection.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LinkedOrderedSet(int capacity, IEqualityComparer<T>? comparer) : base(comparer)
        {
            this._dic = new Dictionary<ValueTuple<T>, LinkedListNode<T>>(capacity, new InternalEqualityComparer<T>(comparer));
            this._list = new LinkedList<T>();
        }

        /// <summary>
        /// Constructs an instance with the elements specified in the source.
        /// </summary>
        /// <param name="src">Source of the initial value.</param>
        /// <param name="comparer">A comparer that compares keys.</param>
        public LinkedOrderedSet(IEnumerable<T> src, IEqualityComparer<T>? comparer) : this(0, comparer)
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
