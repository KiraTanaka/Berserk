using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.GameData
{
    public class PlayerStorage
    {
        private readonly List<Player> _players;

        public PlayerStorage()
        {
            _players = new List<Player>();
        }

        public void Add(Player player)
        {
            _players.Add(player);
        }

        public Player Get(Guid id)
        {
            return _players.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Player> Find()
        {
            return _players.ToArray();
        }
    }
}
