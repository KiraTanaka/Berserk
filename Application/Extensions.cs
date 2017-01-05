using System;
using System.Collections.Generic;
using System.Linq;
using Domain.GameData;

namespace Application
{
    public static class Extensions
    {
        public static IEnumerable<T> SelectInstancesOf<T>(this IEnumerable<Type> types)
            => types.Where(TypeIs<T>).Select(InstanceOf<T>);

        public static bool TypeIs<T>(Type t)
            => typeof(T).IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null;

        public static T InstanceOf<T>(Type t) 
            => (T)Activator.CreateInstance(t);

        public static bool Contains(this IEnumerable<Player> players, Guid playerGuid)
            => players.Any(x => x.Id == playerGuid);
    }
}