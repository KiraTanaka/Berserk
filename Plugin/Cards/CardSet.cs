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
                    Id = "13",
                    Name = "Мушкетер",
                    Cost = Currency.Gold(6),
                    Element = ElementEnum.Steppe,
                    Health = 8,
                    Range = 1,
                    Attack = new Attack(1, 1, 1)
                },
                new CreatureCard
                {
                    Id = "26",
                    Name = "Хольд",
                    Cost = Currency.Silver(6),
                    Element = ElementEnum.Mount,
                    Health = 9,
                    Range = 1,
                    Attack = new Attack(2, 3, 4)
                },
                new CreatureCard
                {
                    Id = "46",
                    Name = "Гвардия Талиса",
                    Cost = Currency.Gold(3),
                    Element = ElementEnum.Forest,
                    Health = 8,
                    Range = 1,
                    Attack = new Attack(2, 2, 3)
                },
                new CreatureCard
                {
                    Id = "62",
                    Name = "Дарвлата",
                    Cost = Currency.Silver(7),
                    Element = ElementEnum.Swamp,
                    Health = 13,
                    Range = 1,
                    Attack = new Attack(3, 4, 5)
                },
                new CreatureCard
                {
                    Id = "75",
                    Name = "Кромешник",
                    Cost = Currency.Silver(4),
                    Element = ElementEnum.Dark,
                    Health = 8,
                    Range = 1,
                    Attack = new Attack(1, 3, 3)
                },
                new CreatureCard
                {
                    Id = "91",
                    Name = "Пронзающая тучи",
                    Cost = Currency.Gold(3),
                    Element = ElementEnum.Fire,
                    Health = 7,
                    Range = 1,
                    Attack = new Attack(1, 2, 2)
                },
                new CreatureCard
                {
                    Id = "113",
                    Name = "Оруженосец",
                    Cost = Currency.Silver(4),
                    Element = ElementEnum.None,
                    Health = 8,
                    Range = 1,
                    Attack = new Attack(1, 2, 2)
                }
            };
        }
    }
}
