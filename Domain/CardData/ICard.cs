using System;
using System.Drawing;

namespace Domain.CardData
{
    /// <summary>
    /// Любая карта. Реализация определяется в плагине.
    /// </summary>
    public interface ICard
    {
        Guid Id { get; set; }
        string Name { get; set; }
        int Cost { get; set; }
        int Health { get; set; }
        int Range { get; set; }
        Attack Attack { get; set; }
        CardAction Action { get; set; }
        bool Closed { get; set; }
        Guid PlayerGuid { get; set; }
        Point Position { get; set; }
    }
}
