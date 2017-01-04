using System;
using Domain.CardData;

namespace Domain.BoardData
{
    /// <summary>
    /// Любое хранилище для карт игрока.
    /// Предполагается колода, кладбише, вспомогательная зона
    /// </summary>
    public class PlayerCardSet
    {
        public Guid PlayerId { get; }
        private readonly CardSet _cardSet;

        public PlayerCardSet(Guid playerId)
        {
            PlayerId = playerId;
            _cardSet = new CardSet();
        }

        public void Push(IBaseCard card)
        {
            _cardSet.Push(card);
        }

        public IBaseCard Pull()
        {
            return _cardSet.Pull();
        }

        public PlayerCardSetInfo GetInfo()
        {
            return new PlayerCardSetInfo(PlayerId, _cardSet.GetSet());
        }
    }
}
