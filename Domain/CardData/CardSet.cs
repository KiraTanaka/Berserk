using System.Collections.Generic;
using System.Linq;
using Domain.BoardData;

namespace Domain.CardData
{
    /// <summary>
    /// Any card storage.
    /// </summary>
    public class CardSet : ICardSet
    {
        private readonly HashSet<IBaseCard> _cards = new HashSet<IBaseCard>();

        public void Push(IBaseCard card)
        {
            _cards.Add(card);
        }
        
        public IBaseCard Pull()
        {
            var last = _cards.LastOrDefault(); // TODO null
            _cards.Remove(last);
            return last;
        }

        public IBaseCard[] GetSet() => _cards.ToArray();
    }
}
