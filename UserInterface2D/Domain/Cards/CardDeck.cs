using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.Linq;
using Infrastructure.Loop;
using Infrastructure.Random;

namespace Domain.Cards
{
    /// <summary>
    /// Колода карт.
    /// </summary>
    public class CardDeck : IEnumerable<Card>
    {
        private List<Card> _cards;
        
        public CardDeck()
        {
            _cards = new List<Card>();
        }

        public CardDeck(IEnumerable<Card> cards)
        {
            _cards = cards.Shuffle().ToList();
        }

        public Card this[int i] => _cards[i];

        public int Count => _cards.Count;
        
        /// <summary>
        /// Добавляет карту в указанное место в колоде.
        /// </summary>
        public void Push(Card card, int index)
        {
            _cards.Insert(index, card);
        }

        /// <summary>
        /// Добавляет карту на верх колоды.
        /// </summary>
        public void PushTop(Card card)
        {
            _cards.Add(card);
        }

        /// <summary>
        /// Добавляет карты на верх колоды.
        /// </summary>
        public void PushTop(IEnumerable<Card> cards)
        {
            cards.ForEach(PushTop);
        }

        /// <summary>
        /// Добавляет карту на низ колоды.
        /// </summary>
        public void PushBottom(Card card)
        {
            Push(card, 0);
        }

        /// <summary>
        /// Добавляет карты на низ колоды.
        /// </summary>
        public void PushBottom(IEnumerable<Card> cards)
        {
            cards.ForEach(PushBottom);
        }

        /// <summary>
        /// Добавляет карту в случайное место в колоде
        /// </summary>
        public void PushRandom(Card card)
        {
            Push(card, RandomHelper.Next(Count));
        }

        /// <summary>
        /// Возвращает карту с указанным id и удаляет ее из колоды.
        /// </summary>
        public Card Pull(int id)//изменила index на id
        {
            var card = _cards.FirstOrDefault(x=>x.Id==id);
            _cards.Remove(card);
            return card;
        }
        /// <summary>
        /// Возвращает карту с указанным id.
        /// </summary>
        public Card GetCardById(int id) 
            => _cards.FirstOrDefault(x => x.Id == id);
        /// <summary>
        /// Возвращает карты с указанными индексами и удаляет их из колоды.
        /// </summary>
        public IEnumerable<Card> Pull(int[] indexes)
        {
            var result = indexes.Select(x => _cards[x]).ToList();
            _cards = _cards.Where((val, i) => !indexes.Contains(i)).ToList();
            return result;
        }

        /// <summary>
        /// Возращает карту с верха колоды и удаляет ее.
        /// </summary>
        public Card PullTop()
        {
            if (Count == 0) throw new ArgumentException("Deck is empty");
            var card = _cards.Last();
            _cards.RemoveAt(Count - 1); // List.Remove сортирует коллекцию
            return card;
        }

        /// <summary>
        /// Возращает указанное количество карт с верха колоды и удаляет их.
        /// </summary>
        public IEnumerable<Card> PullTop(int amount)
        {
            if (amount > Count) throw new ArgumentException($"Deck rest={Count}, required={amount}");
            var result = new List<Card>();
            amount.ForLoop(i => result.Add(PullTop()));
            return result;
        }

        /// <summary>
        /// Возращает случайную карту и удаляет ее из колоды.
        /// </summary>
        public Card PullRandom()
        {
            return Pull(RandomHelper.Next(Count));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Card> GetEnumerator()
        {
            foreach (var card in _cards)
                yield return card;
        }

        public CardDeck Clone()
        {
            return new CardDeck {_cards = _cards};
        }
    }
}