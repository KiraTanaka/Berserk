using System.Linq;
using Domain;

namespace Plugin
{
    // http://berserk.ru/?route=card/card&card_id=95453
    public class Card95453 : Card
    {
        public Card95453()
        {
            Id = 95453;
            Type = CardTypeEnum.Creature;
            Name = "Авантюристка";
            Element = ElementEnum.Neutral;
            Desriprion = "Направленный удар. Опыт в атаке. Первый удар(-). Рывок.";
            Cost = 4;
            Power = 2;
            Health = 2;
            Attack = DoAttack;
            Feature = x => Result.GetError("Нет особенности");
        }

        private static Result DoAttack(GameState state)
        {
            Card actionCard = state.ActionCard;
            if (actionCard.Closed)
                return Result.GetSuccess();

            Card targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return Result.GetError("Нужно выбрать цель");

            targetCard.Hurt(actionCard.Power);
            return Result.GetSuccess();
        }
    }
}
