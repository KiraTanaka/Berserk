using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Plugin
{
    class Helper
    {
        public static Result StandartAttack(GameState state)
        {
            Card actionCard = state.ActionCard;
            if (actionCard.Closed)
                return Result.GetSuccess();

            Card targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return Result.GetError("Нужно выбрать цель");

            targetCard.Hurt(actionCard.Power);
            actionCard.Close();

            if (targetCard.Closed || !targetCard.IsAlive())
                return Result.GetSuccess();

            actionCard.Hurt(targetCard.Power);
            return Result.GetSuccess();
        }
    }
}
