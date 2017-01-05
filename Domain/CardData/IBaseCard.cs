using System;
using System.Drawing;

namespace Domain.CardData
{
    public interface IBaseCard
    {
        Guid PlayerGuid { get; }
        Guid Id { get; }
        string Name { get; }
        Currency Cost { get; }
        ElementEnum Element { get; }
        CardAction Action { get; }
        bool Closed { get; set; }
        Point Position { get; set; }
    }
}
