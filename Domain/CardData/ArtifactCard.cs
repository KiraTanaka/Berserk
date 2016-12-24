using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CardData
{
    public abstract class ArtifactCard : Card, IEvaluable, IFeatureable, IDescriptable
    {
        public Currency Cost { get; }

        public Action Feature { get; }

        public string Description { get; }

        public int Health { get; set; }

        protected ArtifactCard(Currency cost, Action feature, string description)
        {
            Cost = cost;
            Feature = feature;
            Description = description;
        }
    }
}
