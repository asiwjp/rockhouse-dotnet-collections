using System;

namespace RockHouse.Collections.Slots
{
    /// <summary>
    /// Slot is a container class similar to ValueTuple (or Tuple) with item storage state.
    /// </summary>
    /// <typeparam name="T1">The type of the Item1.</typeparam>
    public class Slot<T1> : AbstractSlot
    {
        private T1 _item1;
        private byte _set1 = 0;

        /// <inheritdoc/>
        public override int Count => _set1;

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

        public Slot()
        {
        }

        public Slot(T1 item1)
        {
            Item1 = item1;
        }

        public Slot(Slot<T1> other)
        {
            this._item1 = other._item1;
            this._set1 = other._set1;
        }

        public Slot(Tuple<T1> other)
        {
            Item1 = other.Item1;
        }

        public Slot(ValueTuple<T1> other)
        {
            Item1 = other.Item1;
        }

        public ValueTuple<T1> RemoveAll()
        {
            var res = this.ToValueTuple();
            _item1 = default;
            _set1 = 0;
            return res;
        }

        public Tuple<T1> ToTuple() => new Tuple<T1>(_item1);

        public ValueTuple<T1> ToValueTuple() => new ValueTuple<T1>(_item1);

        #region IComparable
        /// <inheritdoc/>
        public override int CompareTo(ISlot? obj) => InternalSlotUtils.CompareTo(this, obj);
        #endregion

        #region ISlot

        /// <inheritdoc/>
        public override int Length => 1;

        /// <inheritdoc/>
        public override object? this[int index]
        {
            get
            {
                return index switch
                {
                    0 => _item1,
                    _ => throw new ArgumentOutOfRangeException(nameof(index)),
                };
            }
        }

        /// <inheritdoc/>
        public override bool IsFree(int index)
        {
            return index switch
            {
                0 => this._set1 == 0,
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            };
        }
        #endregion

        #region Object
        /// <inheritdoc/>
        public override bool Equals(object? obj) => InternalSlotUtils.Equals(this, obj);

        /// <inheritdoc/>
        public override int GetHashCode() => InternalSlotUtils.GetHashCode(this);

        /// <inheritdoc/>
        public override string ToString() => InternalSlotUtils.ToString(this);
        #endregion
    }
}
