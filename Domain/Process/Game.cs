using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Cards;
using Infrastructure.Loop;

namespace Domain.Process
{
    public abstract class Game
    {
        private readonly IStorage _storage;
        private readonly IRules _rules;
        private readonly List<Card> _cards;

        protected Game(IStorage storage, IRules rules, List<Card> cards)
        {
            _storage = storage;
            _rules = rules;
            _cards = cards;
        }

        public void Run()
        {
            var users = ConnectUsers();
            var players = CreatePlayers(users).ToList();
            ShowPlayers(players);
            // OfferToChangeCards(players);

            while (true)
            {
                if (players.All(x => x.IsAlive()))
                {
                    players.ForEach(x => Move(x.Id, players));
                }
                else
                {
                    var winner = players.FirstOrDefault(x => x.IsAlive());
                    ShowWinner(winner);
                    break;
                }
            }
        }

        private IEnumerable<User> ConnectUsers()
        {
            int id1 = ConnectUser();
            int id2 = ConnectUser();
            User user1 = _storage.FindById<User>(id1).First();
            User user2 = _storage.FindById<User>(id2).First();
            return new[] {user1, user2};
        }

        public abstract int ConnectUser();

        private IEnumerable<Player> CreatePlayers(IEnumerable<User> users)
        {
            var players = new List<Player>();
            users.ForEach(user =>
            {
                var playerCards = user.CardList.Select(id => _cards.FirstOrDefault(x => x.Id == id)?.Clone()).ToList();
                players.Add(new Player(user.Name, playerCards, _rules));
            });
            return players;
        }

        public abstract void ShowPlayers(IEnumerable<Player> players);

        public abstract void OfferToChangeCards(IEnumerable<Player> players);

        public void Move(Guid currentId, IEnumerable<Player> players)
        {
            while (!GetValue(currentId, players).Success) {}
        }

        private MoveResult GetValue(Guid currentId, IEnumerable<Player> players)
        {
            var playersArr = players.ToArray();
            Player movingPlayer = playersArr.First(x => x.Id == currentId);
            Player waitingPlayer = playersArr.First(x => x.Id != currentId);

            ShowInfo(movingPlayer, waitingPlayer);

            Card actionCard = GetActionCard(movingPlayer);
            IEnumerable<Card> targetCards = GetTargetCards(waitingPlayer);
            CardActionEnum actionWay = GetAttackWay();

            InformAboutAttack(actionCard, targetCards, actionWay);

            var state = new GameState
            {
                ActionCard = actionCard,
                TargetCards = targetCards,
                MovingPlayer = movingPlayer,
                WaitingPlayer = waitingPlayer
            };
            MoveResult moveResult = actionCard.Action(actionWay, state);

            movingPlayer.AfterMove();

            ShowActionResult(moveResult);

            return moveResult;
        }

        public abstract void ShowInfo(Player current, Player another);

        public abstract Card GetActionCard(Player actionPlayer);

        public abstract List<Card> GetTargetCards(Player targetPlayer);

        public abstract CardActionEnum GetAttackWay();

        public abstract void InformAboutAttack(
            Card actionCard, IEnumerable<Card> targetCards, CardActionEnum actionWay);

        public abstract void ShowActionResult(MoveResult moveResult);

        public abstract void ShowWinner(Player winner);
    }
}

