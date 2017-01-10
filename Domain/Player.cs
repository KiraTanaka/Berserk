using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Player
    {
        public User User { get; set; }
        public int Money { get; set; }
        public CardDeck Cemetery { get; set; }
        public CardDeck FullDeck { get; set; }
        public CardDeck ActiveDeck { get; set; }
        
        public Player(User user, CardDeck fullDeck, IRules rules)
        {
            User = user;
            Money = rules.PlayerMoneyAmount;
            FullDeck = fullDeck;
            ActiveDeck = new CardDeck();
            for (var i = 0; i < rules.PlayerStartActiveDeckSize; i++)
            {
                var card = fullDeck.PullTop();
                if (card == null) break;
                ActiveDeck.PushTop(card);
            }
        }

        public void RedealCards(int[] indexes)
        {
            var removedCards = ActiveDeck.Pull(indexes);
            FullDeck.PushBottom(removedCards);
            var newCards = FullDeck.PullTop(indexes.Length);
            ActiveDeck.PushTop(newCards);
        }
    }
}
