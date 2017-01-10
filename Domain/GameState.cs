using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GameState
    {
        public ICard ActionCard { get; set; }
        public IEnumerable<ICard> TargetCards { get; set; }
        public ICard HiringCard { get; set; }
        public User HiringUser { get; set; }
        public User AnotherUser { get; set; }
    }
}
