﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace RockHouse.Collections.Sets
{
    /// <summary>
    /// An abstract implementation of a ISet/IHashSet. 
    /// </summary>
    /// <typeparam name="T">The type of elements.</typeparam>
    public abstract class AbstractSet<T> : AbstractCollection, IHashSet<T>
    {
        /// <inheritdoc/>
        public override bool IsEmpty { get => this.Count == 0; }

        #region ICollection
        /// <inheritdoc/>
        public abstract int Count { get; }

        /// <inheritdoc/>
        public abstract bool IsReadOnly { get; }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        /// <inheritdoc/>
        public abstract void Clear();

        /// <inheritdoc/>
        public abstract bool Contains(T item);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            var availables = array.Length - arrayIndex;
            if (availables < this.Count)
            {
                throw new ArgumentException(nameof(arrayIndex), $"array is too small. required={this.Count}, availables(based on arrayIndex)={availables}, arrayIndex={arrayIndex}");
            }

            foreach (var entry in this)
            {
                array[arrayIndex] = entry;
                arrayIndex++;

            }
        }

        /// <inheritdoc/>
        public abstract bool Remove(T item);
        #endregion

        #region IEnumerable
        /// <inheritdoc/>
        public abstract IEnumerator<T> GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region ISet
        /// <inheritdoc/>
        public abstract bool Add(T item);

        /// <inheritdoc/>
        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            foreach (var o in other)
            {
                this.Remove(o);
            }
        }

        /// <inheritdoc/>
        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                this.Clear();
                return;
            }

            if (!other.Any())
            {
                this.Clear();
                return;
            }

            ISet<T> otherSet = other as ISet<T>;
            if (otherSet == null)
            {
                otherSet = new HashSet<T>(other);
            }
            foreach (var o in this.ToList())
            {
                if (!otherSet.Contains(o))
                {
                    this.Remove(o);
                }
            }
        }

        /// <inheritdoc/>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (object.ReferenceEquals(this, other))
            {
                return false;
            }

            var thisCount = this.Count;
            var otherCount = 0;
            var contains = 0;

            checked
            {
                foreach (var o in other)
                {
                    ++otherCount;
                    if (this.Contains(o))
                    {
                        ++contains;
                    }
                }
            }
            if (otherCount < thisCount)
            {
                return false;
            }
            return (contains < otherCount);
        }

        /// <inheritdoc/>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (object.ReferenceEquals(this, other))
            {
                return false;
            }

            var otherCount = 0;
            foreach (var o in other)
            {
                ++otherCount;
                if (!this.Contains(o))
                {
                    return false;
                }
            }
            return otherCount < this.Count;
        }

        /// <inheritdoc/>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return true;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            var thisCount = this.Count;
            var otherCount = 0;
            var contains = 0;
            foreach (var o in other)
            {
                ++otherCount;
                if (this.Contains(o))
                {
                    ++contains;
                }
            }
            if (otherCount < thisCount)
            {
                return false;
            }
            return (contains <= otherCount);
        }

        /// <inheritdoc/>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            foreach (var o in other)
            {
                if (!this.Contains(o))
                {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            foreach (var o in other)
            {
                if (this.Contains(o))
                {
                    return true;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            var thisCount = this.Count;
            var matched = new HashSet<T>();
            checked
            {
                foreach (var o in other)
                {
                    if (!this.Contains(o))
                    {
                        return false;
                    }
                    else
                    {
                        matched.Add(o);
                    }
                }
            }
            return matched.Count == thisCount;
        }

        /// <inheritdoc/>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (object.ReferenceEquals(this, other))
            {
                this.Clear();
                return;
            }

            var newVals = new List<T>();
            foreach (var o in other)
            {
                if (this.Contains(o))
                {
                    this.Remove(o);
                }
                else
                {
                    newVals.Add(o);
                }
            }
            foreach (var o in newVals)
            {
                this.Add(o);
            }
        }

        /// <inheritdoc/>
        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (object.ReferenceEquals(this, other))
            {
                return;
            }

            var newVals = new List<T>();
            foreach (var o in other)
            {
                if (!this.Contains(o))
                {
                    newVals.Add(o);
                }
            }
            foreach (var o in newVals)
            {
                this.Add(o);
            }
        }
        #endregion

        #region Object
        /// <inheritdoc/>
        public override string ToString()
        {
            return "Count=" + this.Count;
        }
        #endregion

    }
}
