using System;
using System.Collections.Generic;

namespace Domain.CardData
{
    public class Feature
    {
        public string Description { get; set; }
        public Action<IEnumerable<IBaseCard>> Action { get; set; }
    }
}