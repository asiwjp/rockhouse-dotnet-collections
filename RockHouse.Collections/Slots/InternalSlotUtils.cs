using System;
using System.Collections.Generic;
using System.Text;

namespace RockHouse.Collections.Slots
{
    internal static class InternalSlotUtils
    {
        public static bool Equals(ISlot? slot1, object? slot2)
        {
            if (object.ReferenceEquals(slot1, slot2))
            {
                return true;
            }

            if (slot1 == null || slot2 == null)
            {
                return false;
            }

            if (!(slot2 is ISlot slot2Slot))
            {
                return false;
            }

            if (slot1.Length != slot2Slot.Length)
            {
                return false;
            }

            for (var i = 0; i < slot1.Length; ++i)
            {
                if (!object.Equals(slot1[i], slot2Slot[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static int GetHashCode(ISlot? slot)
        {
            if (slot == null)
            {
                throw new ArgumentNullException(nameof(slot));
            }

            var hash = slot[0]?.GetHashCode() ?? 0;
            var i = 1;
            var last = slot.Length;
            while (i < last)
            {
                hash = HashCode.Combine(hash, slot[i]);
                ++i;
            }
            return hash;
        }

        public static int CompareTo(ISlot? slot1, ISlot? slot2)
        {
            if (slot1 == null && slot2 == null)
            {
                return 0;
            }

            if (slot1 == null)
            {
                return -1;
            }

            if (slot2 == null)
            {
                return 1;
            }

            if (slot1.Length != slot2.Length)
            {
                throw new ArgumentException("Length is missmatch.");
            }

            var comparer = Comparer<object>.Default;
            for (var i = 0; i < slot1.Length; ++i)
            {
                var cmp = comparer.Compare(slot1[i], slot2[i]);
                if (cmp != 0)
                {
                    return cmp;
                }
            }
            return 0;
        }

        public static string ToString(ISlot? slot)
        {
            if (slot == null)
            {
                return "";
            }

            var buf = new StringBuilder("(");
            for (var i = 0; i < slot.Length; ++i)
            {
                if (i != 0)
                {
                    buf.Append(", ");
                }
                buf.Append(slot[i]);
            }
            buf.Append(')');
            return buf.ToString();
        }
    }
}
