using System;
using Domain.BoardData;
using Domain.CardData;

namespace Domain.GameData
{
    public interface IPlayerContext
    {
        PlayerMove Move(GameInfo gameInfo, Guid playerId);
        IBaseCard SelectCard(GameInfo gameInfo, ICardSet cardSet, Guid playerId);
    }
}