using System;
using Domain.CardData;

namespace Plugin.Cards
{
    public class CardSet : ICardSet
    {
        public IBaseCard[] GetSet()
        {
            return new IBaseCard[]
            {
                new CreatureCard
                {
                    Id = Guid.NewGuid(),
                    Name = "Мушкетер", // 13
                    Cost = Currency.Gold(6),
                    Element = ElementEnum.Steppe,
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
                new CreatureCard
                {
                    Id = Guid.NewGuid(),//"26",
                    Name = "Хольд",
                    Cost = Currency.Silver(6),
                    Element = ElementEnum.Mount,
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
                new CreatureCard
                {
                    Id = Guid.NewGuid(),//"46",
                    Name = "Гвардия Талиса",
                    Cost = Currency.Gold(3),
                    Element = ElementEnum.Forest,
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
                new CreatureCard
                {
                    Id = Guid.NewGuid(),//"62",
                    Name = "Дарвлата",
                    Cost = Currency.Silver(7),
                    Element = ElementEnum.Swamp,
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
                new CreatureCard
                {
                    Id = Guid.NewGuid(),//"75",
                    Name = "Кромешник",
                    Cost = Currency.Silver(4),
                    Element = ElementEnum.Dark,
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
                new CreatureCard
                {
                    Id = Guid.NewGuid(),//"91",
                    Name = "Пронзающая тучи",
                    Cost = Currency.Gold(3),
                    Element = ElementEnum.Fire,
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
                new CreatureCard
                {
                    Id = Guid.NewGuid(),//"113",
                    Name = "Оруженосец",
                    Cost = Currency.Silver(4),
                    Element = ElementEnum.None,
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
        }
    }
}
