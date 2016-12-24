using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BoardData
{
    /// <summary>
    /// Любое хранилище для карт игрока.
    /// Предполагается колода, кладбише, вспомогательная зона
    /// </summary>
    public class PlayerCardPlace<TCard> : CardPlace<TCard>
    {
        public Guid PlayerId { get; }

        public PlayerCardPlace(Guid playerId)
        {
            PlayerId = playerId;
        }
    }
}
