using System;
using System.Drawing;

namespace Domain.CardData
{
    /// <summary>
    /// Класс для передачи информации о карте клиенту.
    /// Поскольку реализация карты не известна, 
    /// используется как объект для десериализации на стороне клиента.
    /// </summary>
    public class CardInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Health { get; set; }
        public int Range { get; set; }
        public Attack Attack { get; set; }
        public string Action { get; set; }
        public bool Closed { get; set; }
        public Guid PlayerGuid { get; set; }
        public Point Position { get; set; }
        
        public CardInfo() { }

        public CardInfo(ICard card)
        {
            Id = card.Id;
            Name = card.Name;
            Cost = card.Cost;
            Health = card.Health;
            Range = card.Range;
            Attack = card.Attack;
            Action = card.Action?.Description;
            Closed = card.Closed;
            PlayerGuid = card.PlayerGuid;
            Position = card.Position;
        }
    }
}
