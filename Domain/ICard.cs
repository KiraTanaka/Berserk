using System;

namespace Domain
{
    public interface ICard
    {
        int Id { get; set; }
        CardTypeEnum Type { get; set; }
        string Name { get; set; }
        ElementEnum Element { get; set; }
        string Desriprion { get; set; }
        int Cost { get; set; }
        int Power { get; set; }
        int Health { get; set; }
        Func<GameState, Result> Hire { get; set; }
        Func<GameState, Result> Attack { get; set; }
        Func<GameState, Result> Feature { get; set; }
        string EquipementType { get; set; }
        bool Closed { get; set; }
        ICard Clone();
    }
}
