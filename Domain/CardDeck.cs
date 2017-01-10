using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace Domain
{
    /// <summary>
    /// Колода карт.
    /// </summary>
    public class CardDeck : IEnumerable<ICard>
    {
        private List<ICard> _cards;
        
        public CardDeck()
        {
            _cards = new List<ICard>();
        }

        public CardDeck(IEnumerable<ICard> cards)
        {
            _cards = cards.Shuffle().ToList();
        }

        public ICard this[int i] => _cards[i];

        /// <summary>
        /// Остаток карт в колоде.
        /// </summary>
        public int Rest => _cards.Count;

        /// <summary>
        /// Добавляет карту в указанное место в колоде.
        /// </summary>
        public void Push(ICard card, int index)
        {
            _cards.Insert(index, card);
        }

        /// <summary>
        /// Добавляет карту на верх колоды.
        /// </summary>
        public void PushTop(ICard card)
        {
            _cards.Add(card);
        }

        /// <summary>
        /// Добавляет карты на верх колоды.
        /// </summary>
        public void PushTop(IEnumerable<ICard> cards)
        {
            cards.ForEach(PushTop);
        }

        /// <summary>
        /// Добавляет карту на низ колоды.
        /// </summary>
        public void PushBottom(ICard card)
        {
            Push(card, 0);
        }

        /// <summary>
        /// Добавляет карты на низ колоды.
        /// </summary>
        public void PushBottom(IEnumerable<ICard> cards)
        {
            cards.ForEach(PushBottom);
        }

        /// <summary>
        /// Добавляет карту в случайное место в колоде
        /// </summary>
        public void PushRandom(ICard card)
        {
            var index = RandomHelper.Next(Rest);
            Push(card, index);
        }

        /// <summary>
        /// Возвращает карту с указанным индексом и удаляет ее из колоды.
        /// </summary>
        public ICard Pull(int index)
        {
            var card = _cards[index];
            _cards.RemoveAt(index);
            return card;
        }

        /// <summary>
        /// Возвращает карты с указанными индексами и удаляет их из колоды.
        /// </summary>
        public IEnumerable<ICard> Pull(int[] indexes)
        {
            var result = indexes.Select(x => _cards[x]).ToList();
            _cards = _cards.Where((val, i) => !indexes.Contains(i)).ToList();
            return result;
        }

        /// <summary>
        /// Возращает карту с верха колоды и удаляет ее.
        /// </summary>
        public ICard PullTop()
        {
            if (Rest == 0) throw new ArgumentException("Deck is empty");
            var card = _cards.Last();
            _cards.RemoveAt(Rest - 1); // List.Remove сортирует коллекцию
            return card;
        }

        /// <summary>
        /// Возращает указанное количество карт с верха колоды и удаляет их.
        /// </summary>
        public IEnumerable<ICard> PullTop(int amount)
        {
            if (amount > Rest) throw new ArgumentException($"Deck rest={Rest}, required={amount}");
            var result = new List<ICard>();
            amount.ForEach(() => result.Add(PullTop()));
            return result;
        }

        /// <summary>
        /// Возращает случайную карту и удаляет ее из колоды.
        /// </summary>
        public ICard PullRandom()
        {
            var index = RandomHelper.Next(Rest);
            return Pull(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ICard> GetEnumerator()
        {
            foreach (var card in _cards)
                yield return card;
        }
    }
}