using System;
using Domain.CardData;

namespace Domain.BoardData
{
    /// <summary>
    /// Класс для передачи информации о зоне пользователя клиенту.
    /// </summary>
    public class PlayerZoneInfo
    {
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public CardInfo[] Desk { get; set; }
        public CardInfo[] UtilityArea { get; set; }
        public CardInfo[] Cementery { get; set; }
        public int Currency { get; set; }
    }
}
