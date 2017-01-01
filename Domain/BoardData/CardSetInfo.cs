using Domain.CardData;

namespace Domain.BoardData
{
    public struct CardSetInfo
    {
        public ICard[] Cards { get; }

        public CardSetInfo(ICard[] cards)
        {
            Cards = cards;
        }
    }
}
