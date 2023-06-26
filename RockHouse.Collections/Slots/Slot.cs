namespace RockHouse.Collections.Slots
{
    public static class Slot
    {
        /// <summary>
        /// Create instance by the specified arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the item1.</typeparam>
        /// <param name="item1">Initial value of Item1.</param>
        /// <returns>New Slot instance.</returns>
        public static Slot<T1> Create<T1>(T1 item1)
        {
            return new Slot<T1>(item1);
        }

        /// <summary>
        /// Create instance by the specified arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the item1.</typeparam>
        /// <typeparam name="T2">The type of the item2.</typeparam>
        /// <param name="item1">Initial value of Item1.</param>
        /// <param name="item2">Initial value of Item2.</param>
        /// <returns>New Slot instance.</returns>
        public static Slot<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Slot<T1, T2>(item1, item2);
        }

    }
}
