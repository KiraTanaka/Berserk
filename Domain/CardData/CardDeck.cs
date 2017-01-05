using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.CardData
{
    /// <summary>
    /// Any card storage.
    /// </summary>
    public class CardDeck : ICardSet
    {
        private readonly HashSet<HashSetCardWrapper> _cards; // TODO карты могут повторяться
        private readonly Random _random = new Random();

        public CardDeck(IEnumerable<IBaseCard> cards)
        {
            _cards = new HashSet<HashSetCardWrapper>();
            foreach (var card in cards)
                _cards.Add(new HashSetCardWrapper(card));
        }

        public int Rest => _cards.Count;

        public void Push(IBaseCard card)
        {
            _cards.Add(new HashSetCardWrapper(card));
        }
        
        public IBaseCard Pull()
        {
            var card = _cards.LastOrDefault(); // TODO null
            return Remove(card);
        }

        public IBaseCard PullRandom()
        {
            var card = _cards.ElementAtOrDefault(_random.Next(_cards.Count));
            return Remove(card);
        }

        private IBaseCard Remove(HashSetCardWrapper card)
        {
            _cards.Remove(card);
            return card.Card;
        }

        public CardDeck[] SplitRandom(int parts)
        {
            var splitted = new List<List<IBaseCard>>();
            for (var i = 0; i < parts; i++) splitted.Add(new List<IBaseCard>());
            for (var i = 0; i < Rest;  i++) splitted.ForEach(x => x.Add(PullRandom()));
            return splitted.Select(x => new CardDeck(x)).ToArray();
        }

        public IBaseCard[] GetSet() => _cards.Select(x => x.Card).ToArray();

        private class HashSetCardWrapper
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
                var otherCard = (obj as HashSetCardWrapper)?.Card;
                return Card != null
                       && otherCard != null
                       && otherCard.Id == Card.Id
                       && otherCard.Name == Card.Name;
            }
        }
    }
}
