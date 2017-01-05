using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Domain.CardData;

namespace Plugin.Cards
{
    public static class ActionFactory
    {
        public static bool SimpleAttack(
            ICreatureCard attackCard, ICreatureCard defenceCard, AttackEnum attack, DefenceEnum defence)
        {
            if (!DistanceAcceptable(attackCard, defenceCard)) return false;
            if (defence == DefenceEnum.Block) return true;
            if (defence == DefenceEnum.Counterattack)
            {
                defenceCard.Action.SimpleAttack(defenceCard, attackCard, AttackEnum.Low, DefenceEnum.Fail);
            }
            else
            {
                defenceCard.Health = defenceCard.Health - attackCard.Attack[attack];
            }
            attackCard.Closed = true;
            return true;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static bool FeatureAttack13(
            IBaseCard attackCard, IEnumerable<ICreatureCard> defenceCards, AttackEnum attack, DefenceEnum defence)
        {
            if (defenceCards.Any(x => !DistanceAcceptable(attackCard, x))) return false;
            if (defenceCards.GroupBy(x => x.Id).ToList().Count != defenceCards.Count()) return false;
            foreach (var card in defenceCards) card.Health = card.Health - 1;
            attackCard.Closed = true;
            return true;
        }

        private static bool DistanceAcceptable(IBaseCard card1, IBaseCard card2, int distance = 1)
        {
            var xDiff = Math.Abs(card1.Position.X - card2.Position.X);
            var yDiff = Math.Abs(card1.Position.Y - card2.Position.Y);
            return distance == 1 // соседняя по диагонали
                ? xDiff == 1 && xDiff == 1 || xDiff + yDiff <= distance
                : xDiff + yDiff <= distance;
        }
    }
}
