﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Infrastructure;

namespace Domain
{
    /// <summary>
    /// Immutable.
    /// </summary>
    public class Player : ICloneable<Player>
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
        public int Money { get; private set; }

        /// <summary>
        /// Герой.
        /// </summary>
        public Card Hero { get; private set; }

        /// <summary>
        /// Карты на столе.
        /// </summary>
        public Card[] CardsInGame => _cardsInGame.ToArray();
        private ImmutableList<Card> _cardsInGame;

        /// <summary>
        /// Полная колода игрока.
        /// </summary>
        public Card[] FullDeck => _fullDeck.ToArray();
        private CardDeck _fullDeck;

        /// <summary>
        /// Колода активных карт.
        /// </summary>
        public Card[] ActiveDeck => _activeDeck.ToArray();
        private CardDeck _activeDeck;

        /// <summary>
        /// Колода - кладбище.
        /// </summary>
        public Card[] Cemetery => _cemetery.ToArray();
        private CardDeck _cemetery;

        private IRules _rules;

        private Player()
        {
        }
        
        public Player(string name, ICollection<Card> cards, IRules rules)
        {
            Id = Guid.NewGuid();
            Name = name;
            Money = rules.PlayerStartMoneyAmount;

            _rules = rules;

            _cardsInGame = ImmutableList<Card>.Empty;
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
        /// Добавляет игроку денег и возращает его копию.
        /// Количество добавленных денег зависит от правил.
        /// Если количество денег максимально, то оно не увеличится.
        /// Максимальное количество денег зависит от правил.
        /// </summary>
        public Player AddMoney()
        {
            return this.Clone(player =>
            {
                if (_rules.PlayerMaxMoneyAmount < Money)
                    player.Money += _rules.PlayerAddMoneyAmount;
            });
        }

        /// <summary>
        /// Добавляет игроку карт и возращает его копию.
        /// Количество добавленных карт зависит от правил.
        /// </summary>
        public Player AddCards()
        {
            return this.Clone(player =>
            {
                _rules.PlayerAddMoneyAmount.ForLoop(i =>
                {
                    var card = player._fullDeck.PullTop();
                    player._activeDeck.PushTop(card);
                });
            });
        }

        /// <summary>
        /// Заменяет карты с указанными индексами в активной колоде,
        /// на другие карты случайным образом, и возращает копию игрока.
        /// </summary>
        public Player RedealCards(int[] indexes)
        {
            return this.Clone(player =>
            {
                var removedCards = player._activeDeck.Pull(indexes);
                player._fullDeck.PushBottom(removedCards);
                var newCards = player._fullDeck.PullTop(indexes.Length);
                player._activeDeck.PushTop(newCards);
            });
        }

        /// <summary>
        /// Карты с указанными индексами перемещаются из активной колоды в
        /// игровую, и возращается копия игрока.
        /// </summary>
        public Player Hire(int[] indexes)
        {
            return this.Clone(player =>
            {
                var cards = player._activeDeck.Pull(indexes).ToList();
                cards.ForEach(x => Money -= x.Cost);
                if (Money < 0) return this;
                player._cardsInGame.AddRange(cards);
                return player;
            });
        }

        /// <summary>
        /// Проверяет жив ли игрок.
        /// </summary>
        public bool IsAlive()
        {
            return Hero.IsAlive();
        }

        public Player Clone()
        {
            return new Player
            {
                Id = Id,
                Name = Name,
                Money = Money,
                Hero = Hero.Clone(),
                _rules = _rules,
                _cardsInGame = _cardsInGame,
                _fullDeck = _fullDeck.Clone(),
                _activeDeck = _activeDeck.Clone(),
                _cemetery = _cemetery.Clone()
            };
        }
    }
}
