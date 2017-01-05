using System;
using System.Drawing;
using Domain.CardData;

namespace Plugin.Cards
{
    public class CreatureCard : ICreatureCard
    {
        public Guid PlayerGuid { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Currency Cost { get; set; }
        public ElementEnum Element { get; set; }
        public CardAction Action { get; set; }
        public int Health { get; set; }
        public int Range { get; set; }
        public Attack Attack { get; set; }
        public string Moto { get; set; }
        public bool Closed { get; set; }
        public Point Position { get; set; }
    }
}
