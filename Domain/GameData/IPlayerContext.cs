using System;

namespace Domain.GameData
{
    public interface IPlayerContext
    {
        PlayerMove Move(GameInfo gameInfo, Guid playerId);
    }
}