using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.CardData
{
    /// <summary>
    /// Любая колода карт.
    /// </summary>
    public class CardDeck
    {
        private readonly HashSet<HashSetCardWrapper> _cards;
        private readonly Random _random = new Random();

        public CardDeck(IEnumerable<ICard> cards)
        {
            _cards = new HashSet<HashSetCardWrapper>();
            foreach (var card in cards)
                _cards.Add(new HashSetCardWrapper(card));
        }

        public int Rest
            => _cards.Count;

        public CardInfo[] GetInfo()
            => _cards.Select(x => new CardInfo(x.Card)).ToArray();

        public ICard Pull()
        {
            var card = _cards.LastOrDefault(); // TODO null
            return Pull(card);
        }

        public ICard Pull(Guid cardId)
        {
            var card = _cards.FirstOrDefault(x => x.Card?.Id == cardId);
            return Pull(card);
        }

        public ICard PullRandom()
        {
            var card = _cards.ElementAtOrDefault(_random.Next(_cards.Count));
            return Pull(card);
        }

        private ICard Pull(HashSetCardWrapper card)
        {
            _cards.Remove(card);
            return card?.Card;
        }

        public CardDeck[] SplitRandom(int parts)
        {
            var splitted = new List<List<ICard>>();
            for (var i = 0; i < parts; i++) splitted.Add(new List<ICard>());
            for (var i = 0; i < Rest;  i++) splitted.ForEach(x => x.Add(PullRandom()));
            return splitted.Select(x => new CardDeck(x)).ToArray();
        }
        
        private class HashSetCardWrapper
        {
            public readonly ICard Card;

            public HashSetCardWrapper(ICard card)
            {
                Card = card;
            }

            public override int GetHashCode()
                => Card?.Id.GetHashCode() ?? 0;

            public override bool Equals(object obj)
                => (obj as HashSetCardWrapper)?.Card?.Id == Card?.Id;
        }
    }
}
