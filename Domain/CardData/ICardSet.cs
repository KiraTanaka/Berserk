using System.Collections.Generic;

namespace Domain.CardData
{
    /// <summary>
    /// Маркерный интерфейс для загрузки сета из плагина.
    /// </summary>
    public interface ICardSet : IEnumerable<ICard> { }
}
