using System.Linq;
using Domain;

namespace Plugin
{
    public class Card91307 : Card
    {
        // http://berserk.ru/?route=card/card&card_id=91307
        public Card91307()
        {
            Id = 91307;
            Name = "Азенаэль";
            Type = CardTypeEnum.Creature;
            Element = ElementEnum.Neutral;
            Cost = 2;
            Health = 3;
            Power = 4;
            Desriprion = "Последний удар (сражается так, как если бы у существа противника был Первый Удар).";
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

            actionCard.Hurt(targetCard.Power);
            targetCard.Close();

            if (actionCard.Closed || !actionCard.IsAlive())
                return Result.GetSuccess();

            targetCard.Hurt(actionCard.Power);
            return Result.GetSuccess();
        }
    }
}
