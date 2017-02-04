using Domain.Cards;
using Domain.Process;

namespace Plugin
{
    public class Card95380 : Card
    {
        public Card95380()
        {
            Id = 95380;
            Name = "Жряк";
            Type = CardTypeEnum.Creature;
            Element = CardElementEnum.Neutral;
            Desriprion = "";
            Cost = 0;
            Power = 0;
            Health = 3;
            Attack = Helper.StandartAttack;
            Feature = x => MoveResult.GetError("Нет особенности");
        }
    }
}
