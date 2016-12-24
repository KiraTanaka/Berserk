using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Domain.CardData
{
    public abstract class CreatureCard : Card, IEvaluable, IFeatureable, IDescriptable
    {
        public Currency Cost { get; }

        public int Health { get; set; }

        public AttackLevel Attack { get; set; }

        public int Defence { get; set; }

        public int Moves { get; set; }

        public Action Feature { get; }

        public string Description { get; }

        protected CreatureCard(Currency cost, Action feature, string description)
        {
            Cost = cost;
            Feature = feature;
            Description = description;
        }
    }
}
