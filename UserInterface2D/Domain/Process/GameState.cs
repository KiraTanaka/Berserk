using System.Collections.Generic;
using Domain.Cards;

namespace Domain.Process
{
    public class GameState
    {
        public Card ActionCard { get; set; }
        public IEnumerable<Card> TargetCards { get; set; }
        public Card HiringCard { get; set; }
        public Player MovingPlayer { get; set; }
        public Player WaitingPlayer { get; set; }
    }
}
