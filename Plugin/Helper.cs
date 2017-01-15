﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Cards;
using Domain.Process;

namespace Plugin
{
    class Helper
    {
        public static MoveResult StandartAttack(GameState state)
        {
            Card actionCard = state.ActionCard;
            if (actionCard.Closed)
                return MoveResult.GetSuccess();

            Card targetCard = state.TargetCards.FirstOrDefault();
            if (targetCard == null)
                return MoveResult.GetError("Нужно выбрать цель");

            targetCard.Hurt(actionCard.Power);
            actionCard.Close();

            if (targetCard.Closed || !targetCard.IsAlive())
                return MoveResult.GetSuccess();

            actionCard.Hurt(targetCard.Power);
            return MoveResult.GetSuccess();
        }
    }
}
