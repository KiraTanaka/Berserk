using System;
using System.Linq;
using Domain;
using Domain.Cards;
using Domain.GameProcess;

namespace Plugin
{
    public class Card87242 : Card
    {
        public Card87242()
        {
            CardId = 87242;
            Type = CardTypeEnum.Creature;
            Name = "Послушник";
            Element = CardElementEnum.Neutral;
            Desriprion = "Найм: Выстрел по существу на 4.";
            Cost = 5;
            Power = 2;
            Health = 3;
            Attack = Helper.StandartAttack;
            Feature = x => MoveResult.GetError("Нет особенности");
        }
    }
}
