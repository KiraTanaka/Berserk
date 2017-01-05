using System;
using System.Collections.Generic;
using System.Threading;
using Domain.GameData;
using Newtonsoft.Json;

namespace Application.Net
{
    public class HttpGameContext : IGameContext
    {
        private readonly List<Player> _players = new List<Player>();
        private ContextState _state;

        public string ParseRequest(string request)
        {
            var userRequest = JsonConvert.DeserializeObject<Request>(request);
            if (userRequest == null)
                return Responce.Error("Incorrect request").ToJson();
            if (userRequest.Registration)
                return GetRegistrationResult(userRequest);
            if (!_players.Contains(userRequest.UserId))
                return Responce.Error("Unknown player").ToJson();
            if (_state.ExpectedPlayer != userRequest.UserId)
                return Responce.Error("Another player is expected").ToJson();
            if (_state.Stage == StageEnum.SelectCard)
                return GetChooseCardResult(userRequest);
            if (_state.Stage == StageEnum.Move)
                return GetMoveResult(userRequest);
            return Responce.Error("Incorrect request").ToJson();
        }

        private string GetRegistrationResult(Request request)
        {
            if (_players.Count == 2)
                return Responce.Error("All players are registered").ToJson();

            var player = new Player(request.UserId, request.Name, this);
            _players.Add(player);

            return _players.Count != 2
                ? Responce.Success($"ExpectedPlayer {player} registered").ToJson()
                : Responce.Success($"ExpectedPlayer {player} registered. Game is started").ToJson();
        }

        private string GetChooseCardResult(Request userRequest)
        {
            _state.SelectedCard = userRequest.SelectedCard;

            return _state.SelectedCard == null 
                ? Responce.Error("Unknown card", _state.GameInfo).ToJson() 
                : Responce.Success($"SelectedCard {userRequest.SelectedCard} chosen").ToJson();
        }

        private string GetMoveResult(Request userRequest)
        {
            if (userRequest.Move == null)
                return Responce.Error("Please move", _state.GameInfo).ToJson();
           // _moves.Add(userRequest.Move);
            return Responce.Success("Moved").ToJson();
        }

        // IGameContext

        public Guid SelectCard(Guid player, GameInfo gameInfo)
        {
            if (!_players.Contains(player)) throw new ArgumentException("Unknown player");

            _state.GameInfo = gameInfo;
            _state.ExpectedPlayer = player;
            _state.Stage = StageEnum.SelectCard;

            return ReturnWhenReady(
                () => _state.ExpectedPlayer == player,
                () => _state.SelectedCard);
        }

        public PlayerMove Move(Guid player, GameInfo gameInfo)
        {
            if (!_players.Contains(player)) throw new ArgumentException("Unknown player");

            _state.GameInfo = gameInfo;
            _state.ExpectedPlayer = player;
            _state.Stage = StageEnum.Move;

            while (true)
            {
            }
        }

        public IEnumerable<Player> GetPlayers()
        {
            return ReturnWhenReady(() => _players.Count == 2, () => _players);
        }

        private static T ReturnWhenReady<T>(Func<bool> condition, Func<T> execute)
        {
            while (true)
            {
                Thread.Sleep(200);
                if (!condition()) continue;
                return execute();
            }
        }

        private struct ContextState
        {
            public GameInfo GameInfo { get; set; }
            public Guid ExpectedPlayer { get; set; }
            public PlayerMove Move { get; set; }
            public Guid SelectedCard { get; set; }
            public StageEnum Stage { get; set; }
        }
    }
}
