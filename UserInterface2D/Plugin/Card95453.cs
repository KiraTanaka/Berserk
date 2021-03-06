﻿using System.Linq;
using Domain;
using Domain.Cards;
using Domain.GameProcess;

namespace Plugin
{
    // http://berserk.ru/?route=card/card&card_id=95453
    public class Card95453 : Card
    {
        public Card95453()
        {
            CardId = 95453;
            Type = CardTypeEnum.Creature;
            Name = "Авантюристка";
            Element = CardElementEnum.Neutral;
            Desriprion = "Направленный удар. Опыт в атаке. Первый удар(-). Рывок.";
            Cost = 4;
            Power = 2;
            Health = 2;
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

            targetCard.Hurt(actionCard.Power);
            return MoveResult.GetSuccess();
        }
    }
}
