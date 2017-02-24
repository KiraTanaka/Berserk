using System.Collections.Generic;
using System.Linq;
using Domain.Cards;
using Domain.Tools;
using System;

namespace Domain.GameProcess
{
    public abstract class Game
    {
        public GameState State { get; } = new GameState();

        private readonly IList<Player> _players;


        protected Game(IRules rules, IEnumerable<Card> cards, UserLimitedSet users)
        {
            _players = new List<Player>();
            users.ForEach(user =>
            {
                var playerCards = user
                    .CardList.Select(cardId =>
                        Card.CreateInstance(cards.FirstOrDefault(x =>
                            x.CardId == cardId)))
                    .ToList();

                _players.Add(new Player(user.Name, playerCards, rules));
            });
        }


        public void Run()
        {
            State.MovingPlayer = _players.First();
            State.WaitingPlayer = _players.Last();
            State.TargetCards = new List<Card>();

            SubscribeToEventsOfActiveCard(State.MovingPlayer.Hero);
            SubscribeToEventsOfActiveCard(State.WaitingPlayer.Hero);
        }
        
        public void Move(CardActionEnum actionWay)
        {
            State.ActionCard.Action(actionWay, State);
            AfterMove();
        }

        public void AfterMove()
        {
            State.MovingPlayer.AfterMove();
        }

        public void StartStep()
        {
            var movingPlayer = State.MovingPlayer;
            var waitingPlayer = State.WaitingPlayer;

            movingPlayer.StartStep();
            onOpenAll?.Invoke(State.MovingPlayer.Id);
            onUpdateCountCoins?.Invoke(movingPlayer.Id, movingPlayer.Money.Count);
            onUpdateCardsInHand?.Invoke(movingPlayer.Id, movingPlayer.ActiveDeck.ToList());
        }

        public void CompleteStep(Guid playerId)
        {
            if (State.MovingPlayer.Id != playerId) return;
            ChangeOfMovingPlayer();
            StartStep();
        }

        public void ChangeOfMovingPlayer()
        {
            Player waitingPlayer = State.MovingPlayer;
            State.MovingPlayer = State.WaitingPlayer;
            State.WaitingPlayer = waitingPlayer;
        }

        public void HireEntity(Guid playerId, Guid instId)
        {
            var movingPlayer = State.MovingPlayer;
            if (playerId != movingPlayer.Id) return;
            bool result = State.MovingPlayer.Hire(instId);
            if(!result) return;
            var card = movingPlayer.CardOnField.FirstOrDefault(x => x.InstId == instId);
            
            onCloseCoins?.Invoke(card.Cost, movingPlayer.Id);
            onCreateActiveCard?.Invoke(card, movingPlayer.Id);
            onDestroyCardInHand?.Invoke(instId, movingPlayer.Id);

            SubscribeToEventsOfActiveCard(card);
        }
        private void SubscribeToEventsOfActiveCard(Card card)
        {
            card.OnChangeHealth += ChangeHealthCard;
            card.OnChangeClosed += ChangeClosedCard;
        }
        private void ChangeHealthCard(Guid instId, int health) => onChangeHealthCard?.Invoke(instId, health);
        private void ChangeClosedCard(Guid instId, bool closed) => onChangeClosedCard?.Invoke(instId, closed);

        #region delegates and events

        public delegate void OnOpenAll(Guid playerId);       
        public delegate void OnCloseCoins(int count, Guid playerId);
        public delegate void OnChangeHealthCard(Guid instId, int health);
        public delegate void OnCreateActiveCard(Card card, Guid playerId);
        public delegate void OnChangeClosedCard(Guid instId, bool closed);
        public delegate void OnDestroyCardInHand(Guid instId, Guid playerId);         
        public delegate void OnUpdateCardsInHand(Guid playerId, List<Card> cards);
        public delegate void OnUpdateCountCoins(Guid playerId, int countCoinsPlayer);
        public event OnOpenAll onOpenAll;
        public event OnCloseCoins onCloseCoins;
        public event OnCreateActiveCard onCreateActiveCard;                       
        public event OnUpdateCountCoins onUpdateCountCoins;
        public event OnChangeHealthCard onChangeHealthCard;
        public event OnChangeClosedCard onChangeClosedCard;
        public event OnDestroyCardInHand onDestroyCardInHand;
        public event OnUpdateCardsInHand onUpdateCardsInHand;

        #endregion
    }
}
