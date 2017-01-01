using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BoardData
{
    public class BattleField
    {
        public int[,] Field { get; }

        public BattleField(int rows, int columns)
        {
            Field = new int[rows, columns];
        }

        public BattleFieldInfo GetInfo()
        {
            return new BattleFieldInfo();
        }
    }
}
