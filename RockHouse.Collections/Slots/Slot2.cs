using System;

namespace RockHouse.Collections.Slots
{
    /// <summary>
    /// Slot is a container class similar to ValueTuple (or Tuple) with item storage state.
    /// </summary>
    /// <typeparam name="T1">The type of the Item1.</typeparam>
    /// <typeparam name="T2">The type of the Item2.</typeparam>
    public class Slot<T1, T2> : AbstractSlot
    {
        private T1 _item1;
        private T2 _item2;
        private byte _set1 = 0;
        private byte _set2 = 0;

        /// <inheritdoc/>
        public override int Count { get => _set1 + _set2; }

        /// <summary>
        /// The first element.
        /// </summary>
        public T1 Item1
        {
            get
            {
                return _item1;
            }
            set
            {
                _item1 = value;
                _set1 = 1;
            }
        }

        /// <summary>
        /// The second element.
        /// </summary>
        public T2 Item2
        {
            get
            {
                return _item2;
            }
            set
            {
                _item2 = value;
                _set2 = 1;
            }
        }

        /// <summary>
        /// Constructs an instance with default values.
        /// </summary>
        public Slot()
        {
        }

        /// <summary>
        /// Constructs an instance with the specified arguments.
        /// </summary>
        /// <param name="item1">Initial value of Item1.</param>
        /// <param name="item2">Initial value of Item2.</param>
        public Slot(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        /// <summary>
        /// Constructs an instance with the specified arguments.
        /// </summary>
        /// <param name="other">Source of the initial value.</param>
        public Slot(Slot<T1, T2> other)
        {
            this._item1 = other._item1;
            this._item2 = other._item2;
            this._set1 = other._set1;
            this._set2 = other._set2;
        }

        /// <summary>
        /// Constructs an instance with the specified arguments.
        /// </summary>
        /// <param name="other">Source of the initial value.</param>
        public Slot(Tuple<T1, T2> other)
        {
            Item1 = other.Item1;
            Item2 = other.Item2;
        }

        /// <summary>
        /// Constructs an instance with the specified arguments.
        /// </summary>
        /// <param name="other">Source of the initial value.</param>
        public Slot(ValueTuple<T1, T2> other)
        {
            Item1 = other.Item1;
            Item2 = other.Item2;
        }

        /// <summary>
        /// Remove and empty.
        /// </summary>
        /// <returns>ValueTuple that contains the removed items.</returns>
        public ValueTuple<T1, T2> RemoveAll()
        {
            var res = this.ToValueTuple();
            _item1 = default;
            _item2 = default;
            _set1 = _set2 = 0;
            return res;
        }

        /// <summary>
        /// Convert to Tuple.
        /// </summary>
        /// <returns>Converted result.</returns>
        public Tuple<T1, T2> ToTuple() => new Tuple<T1, T2>(_item1, _item2);

        /// <summary>
        /// Convert to ValueTuple.
        /// </summary>
        /// <returns>Converted result.</returns>
        public ValueTuple<T1, T2> ToValueTuple() => new ValueTuple<T1, T2>(_item1, _item2);

        #region IComparable
        /// <inheritdoc/>
        public override int CompareTo(ISlot? obj) => InternalSlotUtils.CompareTo(this, obj);
        #endregion

        #region ISlot
        /// <inheritdoc/>
        public override bool IsFree(int index)
        {
            return index switch
            {
                0 => this._set1 == 0,
                1 => this._set2 == 0,
                _ => throw new ArgumentOutOfRangeException(nameof(index)),
            };
        }

        /// <inheritdoc/>
        public override void Set(int index, object value)
        {
            switch (index)
            {
                case 0:
                    this.Item1 = (T1)value;
                    break;
                case 1:
                    this.Item2 = (T2)value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            };
        }
        #endregion

        #region ITuple
        /// <inheritdoc/>
        public override int Length => 2;

        /// <inheritdoc/>
        public override object? this[int index]
        {
            get
            {
                return index switch
                {
                    0 => _item1,
                    1 => _item2,
                    _ => throw new ArgumentOutOfRangeException(nameof(index)),
                };
            }
        }
        #endregion

        #region Object
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is Slot<T1, T2> other)
            {
                return object.Equals(_item1, other._item1) && object.Equals(_item2, other._item2);
            }
            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(_item1, _item2);
        }

        /// <inheritdoc/>
        public override string ToString() => InternalSlotUtils.ToString(this);
        #endregion
    }
}
