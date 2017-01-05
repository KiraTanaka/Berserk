using System;
using System.Drawing;

namespace Domain.GameData
{
    public class PlayerMove
    {
        public Guid PlayerId { get; set; }

        public Guid MovingCardId { get; set; }

        public Point TargetCell { get; set; }

        public Guid[] TargetCards { get; set; }


    }
}
