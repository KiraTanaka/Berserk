using System.Collections.Generic;
using System.Linq;

namespace Domain.CardData
{
    /// <summary>
    /// Any card storage.
    /// </summary>
    public class CardDeck : ICardSet
    {
        private readonly HashSet<HashSetCardWrapper> _cards;

        public CardDeck(IEnumerable<IBaseCard> cards)
        {
            _cards = new HashSet<HashSetCardWrapper>();
            foreach (var card in cards)
                _cards.Add(new HashSetCardWrapper(card));
        }

        public void Push(IBaseCard card)
        {
            _cards.Add(new HashSetCardWrapper(card));
        }
        
        public IBaseCard Pull()
        {
            var last = _cards.LastOrDefault(); // TODO null
            _cards.Remove(last);
            return last.Card;
        }

        public IBaseCard[] GetSet() => _cards.Select(x => x.Card).ToArray();

        private struct HashSetCardWrapper
        {
            public readonly IBaseCard Card;

            public HashSetCardWrapper(IBaseCard card)
            {
                Card = card;
            }

            public override int GetHashCode()
            {
                return Card?.Id == null || Card.Name == null
                    ? 0
                    : Card.Id.GetHashCode() * 3 + Card.Name.GetHashCode() * 7;
            }

            public override bool Equals(object obj)
            {
                var otherCard = obj as IBaseCard;
                return Card != null
                       && otherCard != null
                       && otherCard.Id == Card.Id
                       && otherCard.Name == Card.Name;
            }
        }
    }
}
