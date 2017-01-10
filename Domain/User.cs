using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> Cards { get; set; }

        public int Money { get; set; }
        public CardDeck Cemetery { get; set; }
        public CardDeck Deck { get; set; }
    }
}
