using System;

namespace RockHouse.Collections
{
    public abstract class AbstractCollection : IContainer
    {
        protected void CheckEmpty()
        {
            if (this.IsEmpty)
            {
                throw new InvalidOperationException("Collection is empty.");
            }
        }

        #region IContainer
        public abstract bool IsEmpty { get; }
        #endregion
    }
}
