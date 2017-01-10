using System.Linq;
using Domain;

namespace Plugin
{
    public class Card87299 : Card
    {
        public Card87299()
        {
            Id = 87299;
            Name = "Адепт Белого ордена";
            Element = ElementEnum.Neutral;
            Desriprion = "Излечить выбранное открытое существо или " +
                         "выбранного открытого героя на 4, закройте это существо или героя.";
            Cost = 2;
            Power = 1;
            Health = 3;
            Hire = Helper.StandartHiring2;
            Attack = Helper.StandartAttack;
            Feature = state =>
            {
                ICard actionCard = state.ActionCard;
                if (actionCard.Closed)
                    return Result.GetSuccess();

                ICard targetCard = state.TargetCards.FirstOrDefault();
                if (targetCard == null)
                    return Result.GetError("Нужно выбрать цель");

                targetCard.Heal(4);
                targetCard.Close();
                actionCard.Close();

                return Result.GetSuccess();
            };
        }
    }
}
