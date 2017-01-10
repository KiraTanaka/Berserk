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
            Hire = x =>
            {
                Helper.StandartHiring(x);
                return new Result();
            };
            Attack = DoAttack;
            Feature = x => Result.GetError("Нет особенности"); ;
        }

        private static Result DoAttack(GameState state)
        {
            ICard actionCard = state.ActionCard;
            if (actionCard.Closed)
                return Result.GetSuccess();

            ICard targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return Result.GetError("Нужно выбрать цель");

            targetCard.HurtBy(actionCard.Power);
            return Result.GetSuccess();
        }
    }
}
