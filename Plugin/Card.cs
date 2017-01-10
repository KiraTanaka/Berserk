using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Plugin
{
    public class Card : ICard
    {
        public int Id { get; set; }
        public CardTypeEnum Type { get; set; }
        public string Name { get; set; }
        public ElementEnum Element { get; set; }
        public string Desriprion { get; set; }
        public int Cost { get; set; }
        public int Power { get; set; }
        public int Health { get; set; }
        public Func<GameState, Result> Hire { get; set; }
        public Func<GameState, Result> Attack { get; set; }
        public Func<GameState, Result> Feature { get; set; }
        public string EquipementType { get; set; }
        public bool Closed { get; set; }
    }
}
