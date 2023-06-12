using RockHouse.Collections.ReferenceHolders;
using System;

namespace RockHouse.Collections
{
    /// <summary>
    /// A ReferenceStrength indicating the strength of the reference.
    /// </summary>
    public class ReferenceStrength
    {
        /// <summary>
        /// Indicates a strong reference.
        /// </summary>
        public static readonly ReferenceStrength Hard = new HardReferenceStrength();

        /// <summary>
        /// Indicates a weak reference.
        /// </summary>
        public static readonly ReferenceStrength Weak = new WeakReferenceStrength();

        /// <summary>
        /// Returns a Holder that holds the instance specified in the argument, with the reference strength indicated by the object.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        internal virtual AbstractReferenceHolder Hold(object instance)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Indicates a strong reference.
    /// </summary>
    public sealed class HardReferenceStrength : ReferenceStrength
    {
        internal override AbstractReferenceHolder Hold(object instance)
        {
            return new HardReferenceHolder(instance);
        }
    }

    /// <summary>
    /// Indicates a weak reference.
    /// </summary>
    public sealed class WeakReferenceStrength : ReferenceStrength
    {
        internal override AbstractReferenceHolder Hold(object instance)
        {
            return new WeakReferenceHolder(instance);
        }
    }
}
