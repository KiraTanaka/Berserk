using System;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public static class EnumerableTypeExtension
    {
        public static IEnumerable<T> SelectInstancesOf<T>(this IEnumerable<Type> types)
        {
            return types
                .Where(TypeIs<T>)
                .Select(InstanceOf<T>);
        }

        public static bool TypeIs<T>(Type t)
        {
            return typeof(T).IsAssignableFrom(t)
                   && t.GetConstructor(Type.EmptyTypes) != null;
        }

        public static T InstanceOf<T>(Type t)
        {
            return (T)Activator.CreateInstance(t);
        }
    }
}