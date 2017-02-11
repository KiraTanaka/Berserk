using System.Collections.Generic;
using System.Linq;
using Domain.Cards;
using Domain.Tools;


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
        }
        
        public void Move()
        {
            CardActionEnum actionWay = GetAttackWay();
            State.ActionCard.Action(actionWay, State);
            AfterMove();
        }

        public void AfterMove()
        {
            State.MovingPlayer.AfterMove();
        }

        public void StartStep()
        {
            State.MovingPlayer.StartStep();
        }

        public void CompleteStep()
        {
            ChangeOfMovingPlayer();
            StartStep();
        }

        public void ChangeOfMovingPlayer()
        {
            Player waitingPlayer = State.MovingPlayer;
            State.MovingPlayer = State.WaitingPlayer;
            State.WaitingPlayer = waitingPlayer;
        }

        public abstract CardActionEnum GetAttackWay();
    }
}
