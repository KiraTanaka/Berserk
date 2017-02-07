using System.Linq;
using Domain;
using Domain.Cards;
using Domain.Process;

namespace Plugin
{
    public class Card87299 : Card
    {
        public Card87299()
        {
            CardId = 87299;
            Name = "Адепт Белого ордена";
            Element = CardElementEnum.Neutral;
            Desriprion = "Излечить выбранное открытое существо или " +
                         "выбранного открытого героя на 4, закройте это существо или героя.";
            Cost = 2;
            Power = 1;
            Health = 3;
            Attack = Helper.StandartAttack;
            Feature = state =>
            {
                Card actionCard = state.ActionCard;
                if (actionCard.Closed)
                    return MoveResult.GetSuccess();

                Card targetCard = state.TargetCards.FirstOrDefault();
                if (targetCard == null)
                    return MoveResult.GetError("Нужно выбрать цель");

                targetCard.Heal(4);
                targetCard.Close();
                actionCard.Close();

                return MoveResult.GetSuccess();
            };
        }
    }
}
