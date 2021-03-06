﻿using Domain.Cards;
using Domain.GameProcess;

namespace Plugin
{
    public class Card87065 : Card
    {
        public Card87065()
        {
            CardId = 87065;
            Name = "Антар";
            Type = CardTypeEnum.Creature;
            Element = CardElementEnum.Neutral;
            Desriprion = "";
            Cost = 2;
            Power = 2;
            Health = 4;
            Attack = Helper.StandartAttack;
            Feature = x => MoveResult.GetError("Нет особенности");
        }
    }
}
