using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Domain.BoardData;
using Domain.CardData;
using Domain.GameData;
using Newtonsoft.Json;

namespace Application.Net
{
    public class GameContext : IPlayerContext
    {
        private readonly List<Player> _players;
        private readonly HashSet<PlayerMove> _moves;
        private readonly Action<IEnumerable<Player>> _startGame;

        private GameInfo _gameInfo;
        private Guid _playerTurn;
        private ExpectationEnum _expectation;

        public GameContext(Action<IEnumerable<Player>> startGame)
        {
            _players = new List<Player>();
            _moves = new HashSet<PlayerMove>();
            _startGame = startGame;
        }
        
        public ICard SelectCard(GameInfo gameInfo, CardSet cardSet, Guid playerId)
        {
            _playerTurn = playerId;

            throw new NotImplementedException();
        }

        public PlayerMove Move(GameInfo gameInfo, Guid playerId)
        {
            if (_players.All(x => x.Id != playerId))
                throw new ArgumentException("Unknown player");

            _gameInfo = gameInfo;
            _playerTurn = playerId;
            _expectation = ExpectationEnum.Move;

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
            var userRequest = JsonConvert.DeserializeObject<Request>(request);
            if (userRequest == null)
                return Responce.Error("Incorrect request").ToJson();
            if (userRequest.Registration)
                return GetRegistrationResult(userRequest);
            if (_players.All(x => x.Id != userRequest.UserId))
                return Responce.Error("Unknown player").ToJson();
            if (_playerTurn != userRequest.UserId)
                return Responce.Error("Another player's turn").ToJson();
            if (_expectation == ExpectationEnum.ChooseCard)
                return GetChooseCardResult(userRequest);
            if (_expectation == ExpectationEnum.Move)
                return GetMoveResult(userRequest);
            return Responce.Error("Incorrect request").ToJson();
        }

        private string GetRegistrationResult(Request request)
        {
            if (_players.Count == 2)
                return Responce.Error("All players are registered").ToJson();

            var player = new Player(request.UserId, request.Name, this);
            _players.Add(player);

            if (_players.Count != 2)
                return Responce.Success($"Player {player} registered").ToJson();

            _startGame(_players);
            return Responce.Success($"Player {player} registered. Game is started").ToJson();
        }
        
        private string GetChooseCardResult(Request userRequest)
        {
            return userRequest.Card == null 
                ? Responce.Error("Please choose the card", _gameInfo).ToJson() 
                : Responce.Success($"Card {userRequest.Card} chosen").ToJson();
        }

        private string GetMoveResult(Request userRequest)
        {
            if (userRequest.Move == null)
                return Responce.Error("Please move", _gameInfo).ToJson();
            _moves.Add(userRequest.Move);
            return Responce.Success("Moved").ToJson();
        }
    }
}
