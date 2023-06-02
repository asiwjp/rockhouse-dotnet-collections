using System;
using System.Linq;

namespace RockHouse.Collections.Dictionaries.Multi.Json
{
    internal static class Supported
    {
        internal static readonly Type[] ConvertableTypes = new Type[]
        {
            typeof(ListValuedDictionary<,>),
            typeof(ListValuedMap<,>),
            typeof(HashSetValuedDictionary<,>),
        };

        internal static readonly Type[] ConvertableKeyTypes = new Type[]
        {
            typeof(bool),
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(byte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(char),
            typeof(string),

            // TODO
            //typeof(DateTime),
            //typeof(DateTimeOffset),
            //typeof(Enum),
            //typeof(Guid),
            //typeof(Uri),
        };

        internal static void CheckKeyType(Type keyType)
        {
            if (!ConvertableKeyTypes.Contains(keyType))
            {
                throw new NotSupportedException($"The specified key type is not supported. type={keyType.FullName}");
            }
        }
    }
}
