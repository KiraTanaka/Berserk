using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    /// <summary>
    /// Класс-посредник для связи игроков из Domain и API для их управления.
    /// </summary>
    public class PlayerContoller
    {
        private IPlayer _player;
        private IConnection _connection;

        public PlayerContoller(IPlayer player, IConnection connection)
        {
            _player = player;
            _connection = connection;
        }
    }
}
