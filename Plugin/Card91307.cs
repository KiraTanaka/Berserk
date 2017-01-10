using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Hire = Helper.StandartHiring2;
            Attack = DoAttack;
            Feature = x => Result.GetError("Нет особенности");
        }

        private static Result DoAttack(GameState state)
        {
            ICard actionCard = state.ActionCard;
            if (actionCard.Closed)
                return Result.GetSuccess();

            ICard targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return Result.GetError("Нужно выбрать цель");

            actionCard.HurtBy(targetCard.Power);
            targetCard.Close();

            if (actionCard.Closed || !actionCard.IsAlive())
                return Result.GetSuccess();

            targetCard.HurtBy(actionCard.Power);
            return Result.GetSuccess();
        }
    }
}
