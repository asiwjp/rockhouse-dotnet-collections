namespace RockHouse.Collections.Dictionaries
{
    public abstract class AbstractOrderedDictionary<K, V> : AbstractDictionary<K, V>, IOrderedDictionary<K, V> where K : notnull
    {
        #region IOrderedDictionary
        public abstract K FirstKey { get; }

        public abstract K LastKey { get; }
        #endregion
    }
}
