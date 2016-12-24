using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CardData
{
    public class AttackLevel
    {
        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public AttackLevel() : this(0, 0, 0) { }

        public AttackLevel(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
