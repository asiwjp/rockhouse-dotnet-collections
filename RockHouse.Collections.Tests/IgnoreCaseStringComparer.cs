using System.Collections;
using System.Collections.Generic;

namespace Tests
{
    public class IgnoreCaseStringComparer : IEqualityComparer, IEqualityComparer<string>
    {
        public new bool Equals(object? x, object? y)
        {
            return this.Equals((string)x, (string)y);
        }

        public bool Equals(string x, string y)
        {
            if (object.Equals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.ToUpper().Equals(y.ToUpper());
        }

        public int GetHashCode(string obj)
        {
            return obj.ToUpper().GetHashCode();
        }

        public int GetHashCode(object obj)
        {
            return obj.ToString().ToUpper().GetHashCode();
        }
    }
}
