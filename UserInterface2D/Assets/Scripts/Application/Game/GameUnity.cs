using System;
using System.Collections.Generic;
using Domain.Cards;
using Domain.Process;

namespace Assets.Scripts.Application.Game
{
    public class GameUnity : Domain.Process.Game
    {
        private readonly Func<CardActionEnum> _getAttackWay;

        public GameUnity(IRules rules, IEnumerable<Card> cards, UserLimitedSet users, 
            Func<CardActionEnum> getAttackWay) : base(rules, cards, users)
        {
            _getAttackWay = getAttackWay;
        }
    
        public override CardActionEnum GetAttackWay()
        {
            return _getAttackWay();
        }
    }
}