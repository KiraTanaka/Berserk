using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.BoardData
{
    internal static class PlayerCardPlaceCollectionExtension
    {
        public static void AddEmpty<T>(this ICollection<PlayerCardPlace<T>> coll, Guid guid)
        {
            coll.Add(new PlayerCardPlace<T>(guid));
        }

        public static void PushFirst<T>(this ICollection<PlayerCardPlace<T>> coll, Guid guid, T item)
        {
            coll.First(x => x.PlayerId == guid).Push(item);
        }
    }
}