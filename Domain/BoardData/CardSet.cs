using System.Collections.Generic;
using System.Linq;
using Domain.CardData;

namespace Domain.BoardData
{
    /// <summary>
    /// Any card storage.
    /// </summary>
    public class CardSet
    {
        private readonly HashSet<ICard> _cards = new HashSet<ICard>();

        public void Push(ICard card)
        {
            _cards.Add(card);
        }
        
        public ICard Pull()
        {
            var last = _cards.LastOrDefault(); // TODO null
            _cards.Remove(last);
            return last;
        }

        public CardSetInfo GetInfo()
        {
            return new CardSetInfo(_cards.ToArray());
        }
    }
}
