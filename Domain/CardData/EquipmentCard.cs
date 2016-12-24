using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CardData
{
    public abstract class EquipmentCard : Card, IFeatureable
    {
        public Action Feature { get; }

        protected EquipmentCard(Action feature)
        {
            Feature = feature;
        }
    }
}
