using Domain.BoardData;

namespace Domain.GameData
{
    /// <summary>
    /// Класс для передачи информации об игре клиенту.
    /// </summary>
    public class GameInfo
    {
        public PlayerZoneInfo[] PlayerZoneInfos { get; set; }
    }
}
