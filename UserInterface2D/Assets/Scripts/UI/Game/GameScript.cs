using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.Tools;
using Domain.Cards;
using Domain.GameProcess;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Game
{
    public class GameScript : NetworkBehaviour
    {
        private int _playerCount;

        private GameUnity _game;

        
        // Use this for initialization
        // ReSharper disable once UnusedMember.Local
        void Start()
        {
            var types = ImportTypes().ToList();
            var rules = types.SelectInstancesOf<IRules>()?.FirstOrDefault();
            var cards = types.SelectInstancesOf<Card>().ToList();
            var users = GetUsers();

            _game = new GameUnity(rules, cards, users, () => AttackWay);
            _game.Run();
        }

        private static IEnumerable<Type> ImportTypes()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Scripts\\DLL");
            var dllPaths = Directory.GetFiles(path, "*.dll");
            return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.GetExportedTypes());
        }

        private static UserLimitedSet GetUsers()
        {
            var storage = new DummyStorage();
            var user1 = storage.FindById<User>(1).First();
            var user2 = storage.FindById<User>(2).First();
            return new UserLimitedSet { user1, user2 };
        }

        public string ActionCardId
        {
            get { return _game.State.ActionCard?.InstId.ToString() ?? ""; }
            set
            {
                if (value != null)
                {
                    Card card = SearchCardAtPlayers(value);
                    _game.State.ActionCard = card.Closed ? null : card;
                }
                else
                {
                    _game.State.ActionCard = null;
                }
            }
        }

        public string TargetCardId
        {
            get { return _game.State.TargetCards?.FirstOrDefault()?.InstId.ToString(); }
            set
            {
                _game.State.TargetCards = value == null
                    ? null
                    : new List<Card> { SearchCardAtPlayers(value) };
            }
        }

        private CardActionEnum _attackWay;

        public CardActionEnum AttackWay
        {
            get { return _attackWay; }
            set
            {
                _attackWay = value;
                _game.Move();
                SetToZeroCards();
                OnAfterMove?.Invoke();
            }
        }

        public Player GetPlayer()
        {
            if (_playerCount >= 2) _playerCount = 0;

            return _playerCount++ == 0 ? _game.State.MovingPlayer : _game.State.WaitingPlayer;
        }

        public void SetToZeroCards()
        {
            ActionCardId = null;
            TargetCardId = null;
        }

        public GameState GetGameState()
        {
            return _game.State;
        }

        public void CompleteStep(string playerId)
        {
            if (GetGameState().MovingPlayer.Id.ToString() == playerId)
            {
                _game.CompleteStep();
                OnStartStep?.Invoke(GetGameState().MovingPlayer.Id.ToString());
            }
        }

        private Card SearchCardAtPlayers(string instId)
        {
            var state = GetGameState();
            return FindCard(instId, state.MovingPlayer) ?? FindCard(instId, state.WaitingPlayer);
        }

        private static Card FindCard(string instId, Player player)
        {
            Card card = player.CardOnField.FirstOrDefault(x => x.InstId.ToString() == instId);
            return card ?? (player.Hero.InstId.ToString() == instId ? player.Hero : null);
        }

        #region delegates and events
        public delegate void OnAfterMoveHandler();

        public delegate void OnStartStepHandler(string playerId);

        public event OnStartStepHandler OnStartStep;

        public event OnAfterMoveHandler OnAfterMove;
        #endregion
    }
}