using System;
using Domain.CardData;

namespace Domain.BoardData
{
    /// <summary>
    /// Любое хранилище для карт игрока.
    /// Предполагается колода, кладбише, вспомогательная зона
    /// </summary>
    public class PlayerCardDeck
    {
        public Guid PlayerId { get; }
        private readonly CardDeck _cardDeck;

        public PlayerCardDeck(Guid playerId, CardDeck cardDeck)
        {
            PlayerId = playerId;
            _cardDeck = cardDeck;
        }

        public void Push(IBaseCard card)
            => _cardDeck.Push(card);

        public IBaseCard Pull(Guid cardId)
            => _cardDeck.Pull(cardId);

        public IBaseCard Pull()
            => _cardDeck.Pull();

        public PlayerCardSetInfo GetInfo()
            => new PlayerCardSetInfo(PlayerId, _cardDeck.GetSet());
    }
}
