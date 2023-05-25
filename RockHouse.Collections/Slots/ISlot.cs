using System;
using System.Runtime.CompilerServices;

namespace RockHouse.Collections.Slots
{
    /// <summary>
    /// Slot is a container class similar to ValueTuple (or Tuple) with item storage state.
    /// ISlot is the interface that those slots should implement.
    /// </summary>
    public interface ISlot : ITuple, IComparable<ISlot>, IContainer
    {
        /// <summary>
        /// Determines whether or not the specified index is unset.
        /// </summary>
        /// <param name="index">N of the ItemN property.</param>
        /// <returns>true if not set, otherwise false.</returns>
        bool IsFree(int index);

        /// <summary>
        /// Sets a value to the specified index.
        /// </summary>
        /// <param name="index">N of the ItemN property.</param>
        /// <param name="value">The value to be set for the ItemN property.</param>
        void Set(int index, object value);
    }
}
