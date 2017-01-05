using System;
using System.Collections;
using System.Collections.Generic;
using Domain.CardData;

namespace Plugin.Cards
{
    public class CardSet : ICardSet
    {
        private readonly ICard[] _cards =
        {
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Мушкетер", // 13
                Cost = 12,
                Health = 8,
                Range = 1,
                Attack = new Attack(1, 1, 1),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Хольд", // 26
                Cost = 6,
                Health = 9,
                Range = 1,
                Attack = new Attack(2, 3, 4),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Гвардия Талиса", // 46
                Cost = 6,
                Health = 8,
                Range = 1,
                Attack = new Attack(2, 2, 3),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Дарвлата", // 62
                Cost = 7,
                Health = 13,
                Range = 1,
                Attack = new Attack(3, 4, 5),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Кромешник", // 75
                Cost = 4,
                Health = 8,
                Range = 1,
                Attack = new Attack(1, 3, 3),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Пронзающая тучи", // 91
                Cost = 6,
                Health = 7,
                Range = 1,
                Attack = new Attack(1, 2, 2),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            },
            new Card
            {
                Id = Guid.NewGuid(),
                Name = "Оруженосец", // 113
                Cost = 4,
                Health = 8,
                Range = 1,
                Attack = new Attack(1, 2, 2),
                Action = new CardAction
                {
                    Description = "Три выстрела на 1, дальность 3. Только по разным картам.",
                    SimpleAttack = ActionFactory.SimpleAttack,
                    FeatureAttack = ActionFactory.FeatureAttack13
                }
            }
        };

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
