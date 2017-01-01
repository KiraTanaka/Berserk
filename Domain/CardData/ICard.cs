using System;
using System.Drawing;

namespace Domain.CardData
{
    public interface ICard
    {
        Guid PlayerId { get; set; }
        Point Position { get; set; }
    }
}