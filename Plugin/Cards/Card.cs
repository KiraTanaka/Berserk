using System;
using System.Drawing;
using Domain.CardData;

namespace Plugin.Cards
{
    public class Card : ICard
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Health { get; set; }
        public int Range { get; set; }
        public Attack Attack { get; set; }
        public CardAction Action { get; set; }
        public bool Closed { get; set; }
        public Guid PlayerGuid { get; set; }
        public Point Position { get; set; }
    }
}
