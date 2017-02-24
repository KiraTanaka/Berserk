using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Infrastructure
{
    public static class TypeExtensions
    {
        public static IEnumerable<T> SelectInstancesOf<T>(this IEnumerable<Type> types)
            => types.Where(TypeIs<T>).Select(InstanceOf<T>);

        public static bool TypeIs<T>(Type t)
            => typeof(T).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null;

        public static T InstanceOf<T>(Type t)
            => (T)Activator.CreateInstance(t);
    }
}
