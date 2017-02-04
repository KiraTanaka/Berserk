using Domain.Cards;
using Domain.Process;

namespace Plugin
{
    public class Card87065 : Card
    {
        public Card87065()
        {
            Id = 87065;
            Name = "Антар";
            Type = CardTypeEnum.Creature;
            Element = CardElementEnum.Neutral;
            Desriprion = "";
            Cost = 2;
            Power = 2;
            Health = 4;
            Attack = Helper.StandartAttack;
            Feature = x => MoveResult.GetError("Нет особенности");
        }
    }
}
