using System;

namespace Domain.BoardData
{
    public struct PlayerCardSetInfo
    {
        public Guid PlayerId { get; }
        public CardSetInfo CardSetInfo { get; }

        public PlayerCardSetInfo(Guid playerId, CardSetInfo cardSetInfo)
        {
            PlayerId = playerId;
            CardSetInfo = cardSetInfo;
        }
    }
}