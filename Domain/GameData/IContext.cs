using System;
using System.Collections.Generic;

namespace Domain.GameData
{
    /// <summary>
    /// Управляет взаимодействием элементов игры.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Передает информацию об игре и возвращает ход пользователя.
        /// </summary>
        PlayerMove Move(Guid player, GameInfo gameInfo);

        /// <summary>
        /// Передает информацию об игре и возвращает ID карты выбранной пользовтелем.
        /// </summary>
        Guid SelectCard(Guid player, GameInfo gameInfo);

        IEnumerable<Player> GetPlayers();
    }
}