using System.Linq;
using Domain;
using Domain.Cards;
using Domain.Process;

namespace Plugin
{
    public class Card91307 : Card
    {
        // http://berserk.ru/?route=card/card&card_id=91307
        public Card91307()
        {
            CardId = 91307;
            Name = "Азенаэль";
            Type = CardTypeEnum.Creature;
            Element = CardElementEnum.Neutral;
            Cost = 2;
            Health = 3;
            Power = 4;
            Desriprion = "Последний удар (сражается так, как если бы у существа противника был Первый Удар).";
            Attack = DoAttack;
            Feature = x => MoveResult.GetError("Нет особенности");
        }

        private static MoveResult DoAttack(GameState state)
        {
            Card actionCard = state.ActionCard;
            if (actionCard.Closed)
                return MoveResult.GetSuccess();

            Card targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return MoveResult.GetError("Нужно выбрать цель");

            actionCard.Hurt(targetCard.Power);
            targetCard.Close();

            if (actionCard.Closed || !actionCard.IsAlive())
                return MoveResult.GetSuccess();

            targetCard.Hurt(actionCard.Power);
            return MoveResult.GetSuccess();
        }
    }
}
