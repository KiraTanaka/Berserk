using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CardData;

namespace Domain.BoardData
{
    /// <summary>
    /// Any card storage.
    /// </summary>
    public abstract class CardPlace<TCard> // Generic allows to change a card type fluently
    {
        private readonly List<TCard> _cards = new List<TCard>();

        public void Push(TCard card)
        {
            _cards.Add(card);
        }

        public TCard Pull()
        {
            return _cards.LastOrDefault(); // TODO
        }
    }
}
