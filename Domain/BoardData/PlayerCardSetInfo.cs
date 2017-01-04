using System;
using Domain.CardData;

namespace Domain.BoardData
{
    public struct PlayerCardSetInfo
    {
        public Guid PlayerId { get; }
        public IBaseCard[] CardSetInfo { get; }

        public PlayerCardSetInfo(Guid playerId, IBaseCard[] cardSetInfo)
        {
            PlayerId = playerId;
            CardSetInfo = cardSetInfo;
        }
    }
}