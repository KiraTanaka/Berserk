using System;
using System.Collections.Generic;

namespace Domain.CardData
{
    public class CardAction
    {
        public string Description { get; set; }
        public Func<ICreatureCard, ICreatureCard, AttackEnum, DefenceEnum, bool> SimpleAttack { get; set; }
        public Func<IBaseCard, IEnumerable<ICreatureCard>, AttackEnum, DefenceEnum, bool> FeatureAttack { get; set; }
    }
}