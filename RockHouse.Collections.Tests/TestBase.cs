using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using Xunit;

namespace RockHouse.Collections.Tests
{
    public class TestBase
    {
        public static T GetFieldValue<T>(object o, string fieldName)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var fld = o.GetType().GetField(fieldName, bindingFlags);
            if (fld == null)
            {
                throw new ArgumentException($"field not found. fieldName={fieldName}");
            }
            return (T)fld.GetValue(o);
        }

        public DateTimeOffset ToDateTimeOffset(string text)
        {
            return DateTimeOffset.Parse(text);
        }

        public static void AssertEquals(object expected, object actual, string path = "")
        {
            try
            {
                EqualsValue(expected, actual, path);
            }
            catch (DiffException ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        public static void EqualsValue(object o1, object o2, string path = "")
        {
            if (object.Equals(o1, o2))
            {
                return;
            }

            if (o1.GetType() != o2.GetType())
            {
                throw new DiffException(path, "Type is different.");
            }

            if (o1.GetType().IsValueType)
            {
                throw new DiffException(path, "Value is different.");
            }

            var o1enum = o1 as System.Collections.IEnumerable;
            if (o1enum != null)
            {
                EqualsEnumerable(o1enum, o2 as System.Collections.IEnumerable, path);
                return;
            }

            EqualsProps(o1, o2, path);
        }

        public static void EqualsProps(object o1, object o2, string path = "")
        {
            foreach (var prop in o1.GetType().GetProperties())
            {
                var currentPath = BuildPath(path, prop.Name);
                var o1PropVal = prop.GetValue(o1);
                var o2PropVal = prop.GetValue(o2);

                EqualsValue(o1PropVal, o2PropVal, currentPath);
            }
        }

        public static void EqualsEnumerable(System.Collections.IEnumerable enum1, System.Collections.IEnumerable enum2, string path = "")
        {
            var list1 = new List<object>();
            foreach (var e1 in enum1)
            {
                list1.Add(e1);
            }

            var list2 = new List<object>();
            foreach (var e2 in enum2)
            {
                list2.Add(e2);
            }

            if (list1.Count != list2.Count)
            {
                throw new DiffException(path, $"Length is different.");
            }

            for (var i = 0; i < list1.Count; ++i)
            {
                var currentPath = BuildArrayIndexPath(path, i);
                EqualsValue(list1[i], list2[i], currentPath);
            }
        }

        private static string BuildPath(string src, string append)
        {
            if (string.IsNullOrEmpty(src))
            {
                return append;
            }
            return src + "." + append;
        }

        private static string BuildArrayIndexPath(string src, int index)
        {
            var indexer = "[" + index + "]";
            return string.IsNullOrEmpty(src) ? indexer : src + indexer;
        }

        public void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }

    public class DiffException : Exception
    {
        public string Path { get; set; }

        public DiffException(string path, string message) : base(message + $" path={path}")
        {
            this.Path = path;
        }

        public DiffException(string path, string message, Exception innerException) : base(message + $" path={path}", innerException)
        {
            this.Path = path;
        }

        protected DiffException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
