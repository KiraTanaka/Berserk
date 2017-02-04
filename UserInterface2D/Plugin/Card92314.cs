using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Cards;
using Domain.Process;

namespace Plugin
{
    public class Card92314 : Card
    {
        public Card92314()
        {
            Id = 92314;
            Type = CardTypeEnum.Creature;
            Name = "Беррейг";
            Element = CardElementEnum.Neutral;
            Desriprion = "Один раз за ход вы можете бесплатно разыграть существо из руки.";
            Cost = 9;
            Power = 6;
            Health = 11;
            Attack = Helper.StandartAttack;
            Feature = x => MoveResult.GetError("Нет особенности");
        }
    }
}
