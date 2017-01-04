using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CardData
{
    public interface ICardSet
    {
        IBaseCard[] GetSet();
    }
}
