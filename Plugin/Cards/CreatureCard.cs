using System;
using Domain.CardData;

namespace Plugin.Cards
{
    public class CreatureCard : ICreatureCard
    {
        public Guid PlayerGuid { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public Currency Cost { get; set; }
        public ElementEnum Element { get; set; }
        public Feature Feature { get; set; }
        public int Health { get; set; }
        public int Range { get; set; }
        public Attack Attack { get; set; }
        public string Moto { get; set; }
    }
}
