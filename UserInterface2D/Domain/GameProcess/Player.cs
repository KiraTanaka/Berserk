using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Cards;
using Domain.Coins;
using Domain.Tools;

namespace Domain.GameProcess
{
    /// <summary>
    /// Контроллер зоны игрока.
    /// Immutable.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Уникальный идентификатор игрока.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Деньги.
        /// </summary>
        public Money Money { get; private set; } = new Money();

        /// <summary>
        /// Герой.
        /// </summary>
        public Card Hero { get; }

        /// <summary>
        /// Карты на столе.
        /// </summary>
        public Card[] CardOnField => _cardOnField.ToArray();
        private readonly List<Card> _cardOnField;

        /// <summary>
        /// Полная колода игрока.
        /// </summary>
        public Card[] FullDeck => _fullDeck.ToArray();
        private readonly CardDeck _fullDeck;

        /// <summary>
        /// Колода активных карт.
        /// </summary>
        public Card[] ActiveDeck => _activeDeck.ToArray();
        private readonly CardDeck _activeDeck;

        /// <summary>
        /// Колода - кладбище.
        /// </summary>
        public Card[] Cemetery => _cemetery.ToArray();
        private readonly CardDeck _cemetery;

        private readonly IRules _rules;
        
        public Player(string name, ICollection<Card> cards, IRules rules)
        {
            Id = Guid.NewGuid();
            Name = name;
            Money.AddMoney(rules.PlayerStartMoneyAmount);

            _rules = rules;

            _cardOnField = new List<Card>();
            Hero = cards.FirstOrDefault(x => x.Type == CardTypeEnum.Hero);
            cards.Remove(Hero);

            _fullDeck = new CardDeck(cards);
            _cemetery = new CardDeck();
            _activeDeck = new CardDeck();
            for (var i = 0; i < rules.PlayerStartActiveDeckSize; i++)
            {
                var card = _fullDeck.PullTop();
                if (card == null) break;
                _activeDeck.PushTop(card);
            }
        }

        /// <summary>
        /// Заменяет карты с указанными индексами в активной колоде,
        /// </summary>
        public Player RedealCards(int[] indexes)
        {
            var removedCards = _activeDeck.Pull(indexes);
            _fullDeck.PushBottom(removedCards);
            var newCards = _fullDeck.PullTop(indexes.Length);
            _activeDeck.PushTop(newCards);
            return this;
        }

        public bool Hire(Guid instId)
        {
            var card = _activeDeck.GetCardById(instId);
            if (Money.Count - card.Cost < 0) return false;
            Money -= card.Cost;
            _activeDeck.Pull(instId);
            _cardOnField.Add(card);
            return true;
        }

        public void AfterMove()
        {
            var dead = _cardOnField.FindAll(x => x.IsAlive() == false);
            dead.ForEach(x => _cardOnField.Remove(x));            
            _cemetery.PushTop(dead);     
        }

        public void StartStep()
        {
            OpenAll();

            if (_fullDeck.Count != 0)
            {
                var card = _fullDeck.PullTop();
                _activeDeck.PushTop(card);
            }

            if (_rules.PlayerMaxMoneyAmount > Money.Count)
                Money += _rules.PlayerAddMoneyAmount;
        }

        private void OpenAll()
        {
            Money.OpenAll();
            CardOnField.ForEach(card => card.Open());
            Hero.Open();
        }

        /// <summary>
        /// Проверяет жив ли игрок.
        /// </summary>
        public bool IsAlive()
        {
            return Hero.IsAlive();
        }
    }
}
