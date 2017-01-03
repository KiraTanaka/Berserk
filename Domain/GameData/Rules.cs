using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.GameData
{
    public class Rules : IRules
    {
        public int FieldRows { get; set; }
        public int FieldColumns { get; set; }
        public int PlayerCardsAmount { get; set; }
    }
}
