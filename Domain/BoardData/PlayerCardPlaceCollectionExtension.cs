using System;
using System.Collections.Generic;
using System.Linq;
using Domain.CardData;

namespace Domain.BoardData
{
    internal static class PlayerCardPlaceCollectionExtension
    {
        public static void AddNewEmpty(this ICollection<PlayerCardSet> coll, Guid guid)
        {
            coll.Add(new PlayerCardSet(guid));
        }

        public static void PushToPlayer(this ICollection<PlayerCardSet> coll, Guid playerId, ICard card)
        {
            coll.First(x => x.PlayerId == playerId).Push(card);
        }

        public static PlayerCardSetInfo[] GetInfo(this ICollection<PlayerCardSet> coll)
        {
            return coll.Select(x => x.GetInfo()).ToArray();
        }
    }
}