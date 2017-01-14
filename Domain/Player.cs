using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public CardDeck Cemetery { get; set; }
        public CardDeck FullDeck { get; set; }
        public CardDeck ActiveDeck { get; set; }
        public List<ICard> CardsInGame { get; set; }
        public ICard Hero { get; set; }

        private readonly IRules _rules;

        public Player(User user, ICollection<ICard> cards, IRules rules)
        {
            _rules = rules;
            Id = user.Id;
            Name = user.Name;
            Money = rules.PlayerStartMoneyAmount;

            CardsInGame = new List<ICard>();

            Hero = cards.FirstOrDefault(x => x.Type == CardTypeEnum.Hero);
            cards.Remove(Hero);
            FullDeck = new CardDeck(cards);

            ActiveDeck = new CardDeck();
            for (var i = 0; i < rules.PlayerStartActiveDeckSize; i++)
            {
                var card = FullDeck.PullTop();
                if (card == null) break;
                ActiveDeck.PushTop(card);
            }
        }

        public bool AddMoney()
        {
            if (_rules.PlayerMaxMoneyAmount >= Money) return false;
            Money++;
            return true;
        }

        public void DealCard()
        {
            var card = FullDeck.PullTop();
            ActiveDeck.PushTop(card);
        }

        public void RedealCards(int[] indexes)
        {
            var removedCards = ActiveDeck.Pull(indexes);
            FullDeck.PushBottom(removedCards);
            var newCards = FullDeck.PullTop(indexes.Length);
            ActiveDeck.PushTop(newCards);
        }

        public bool IsAlive()
        {
            return Hero.IsAlive();
        }
    }
}
