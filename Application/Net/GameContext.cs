using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.BoardData;
using Domain.GameData;
using Newtonsoft.Json;

namespace Application.Net
{
    public class GameContext : IPlayerContext
    {
        private readonly List<Player> _players;
        private readonly HashSet<PlayerMove> _moves;
        private readonly Action<List<PlayerSet>> _startGame;

        private GameInfo _gameInfo;
        private Guid _playerTurn;

        public GameContext(Action<List<PlayerSet>> startGame)
        {
            _players = new List<Player>();
            _moves = new HashSet<PlayerMove>();
            _startGame = startGame;
        }

        public PlayerMove Move(GameInfo gameInfo, Guid playerId)
        {
            if (_players.All(x => x.Id != playerId))
                throw new ArgumentException("Unknown player");

            _gameInfo = gameInfo;
            _playerTurn = playerId;

            while (true)
            {
                Thread.Sleep(1000);
                var playerMove = _moves.FirstOrDefault(x => x.PlayerId == playerId);
                if (playerMove == null)
                    continue;
                lock (_moves) _moves.Remove(playerMove);
                return playerMove;
            }
        }

        public string ParseRequest(string request)
        {
            var userRequest = JsonConvert.DeserializeObject<UserRequest>(request);
            if (userRequest == null)
            {
                return GameResponce.Error("Incorrect request").ToJson();
            }
            if (userRequest.Registration && _players.Count < 2)
            {
                _players.Add(new Player(userRequest.UserId, userRequest.Name, this));
                if (_players.Count == 2)
                {
                    var playerSets = _players.Select(x => new PlayerSet(x)).ToList();
                    _startGame(playerSets);
                    return GameResponce.Success($"Player {userRequest.Name} is registered. Game is started").ToJson();
                }
                return GameResponce.Success($"Player {userRequest.Name} is registered. Wait for another player").ToJson();
            }
            if (userRequest.Registration && _players.Count >= 2)
            {
                return GameResponce.Error("All players are registered").ToJson();
            }
            if (_players.All(x => x.Id != userRequest.UserId))
            {
                return GameResponce.Error("Unknown player").ToJson();
            }
            if (userRequest.UserId != _playerTurn)
            {
                return GameResponce.Error("Not your turn").ToJson();
            }
            if (userRequest.Move != null)
            {
                lock(_moves) _moves.Add(userRequest.Move);
                return GameResponce.Success("Move added").ToJson();
            }
            return GameResponce.Success(_gameInfo).ToJson();
        }
    }
}
