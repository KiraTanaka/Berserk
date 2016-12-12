using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// Представляет собой игрока. Переопределять в плагинах.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Игрок делает ход и возращает состояние игры после хода.
        /// </summary>
        /// <param name="state">Состояние игры, для принятия игроком решения.</param>
        /// <returns>Новое состояние игры.</returns>
        IState MakeMove(IState state);
    }
}
