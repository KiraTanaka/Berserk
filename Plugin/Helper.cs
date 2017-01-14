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
        public static void StandartHiring(GameState state)
        {
            ICard hiringCard = state.HiringCard;
            Player player = state.MovingPlayer;
            player.Money = player.Money - hiringCard.Cost;
        }

        public static Result StandartHiring2(GameState state)
        {
            StandartHiring(state);
            return Result.GetSuccess();
        }

        public static Result StandartAttack(GameState state)
        {
            ICard actionCard = state.ActionCard;
            if (actionCard.Closed)
                return Result.GetSuccess();

            ICard targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return Result.GetError("Нужно выбрать цель");

            targetCard.HurtBy(actionCard.Power);
            actionCard.Close();

            if (targetCard.Closed || !targetCard.IsAlive())
                return Result.GetSuccess();

            actionCard.HurtBy(targetCard.Power);
            return Result.GetSuccess();
        }
    }
}
