using System.Collections.Generic;

namespace Domain
{
    public class GameState
    {
        public ICard ActionCard { get; set; }
        public IEnumerable<ICard> TargetCards { get; set; }
        public ICard HiringCard { get; set; }
        public Player MovingPlayer { get; set; }
        public Player WaitingPlayer { get; set; }
    }
}
