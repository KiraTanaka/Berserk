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
        private readonly CardDeck _cardDeck;

        public PlayerCardSet(Guid playerId, CardDeck cardDeck)
        {
            PlayerId = playerId;
            _cardDeck = cardDeck;
        }

        public void Push(IBaseCard card)
        {
            _cardDeck.Push(card);
        }

        public IBaseCard Pull()
        {
            return _cardDeck.Pull();
        }

        public PlayerCardSetInfo GetInfo()
        {
            return new PlayerCardSetInfo(PlayerId, _cardDeck.GetSet());
        }
    }
}
