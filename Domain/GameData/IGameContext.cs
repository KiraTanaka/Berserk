using System;
using System.Collections.Generic;
using Domain.CardData;

namespace Domain.GameData
{
    /// <summary>
    /// Управляет взаимодействием элементов игры.
    /// </summary>
    public interface IGameContext
    {
        /// <summary>
        /// Passes an information about the game and returns player's move.
        /// </summary>
        PlayerMove Move(Guid player, GameInfo gameInfo);

        /// <summary>
        /// Passes an information about the game and returns player's selected card guid.
        /// </summary>
        Guid SelectCard(Guid player, GameInfo gameInfo);

        /// <summary>
        /// Returns players of current game.
        /// </summary>
        IEnumerable<Player> GetPlayers();
    }
}