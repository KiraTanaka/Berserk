using System;
using Domain.BoardData;
using Domain.CardData;

namespace Domain.GameData
{
    public interface IPlayerContext
    {
        PlayerMove Move(GameInfo gameInfo, Guid playerId);
        ICard SelectCard(GameInfo gameInfo, CardSet cardSet, Guid playerId);
    }
}