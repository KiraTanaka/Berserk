namespace Domain.GameData
{
    /// <summary>
    /// Основные ограничения игры. Реализуются в плагинах.
    /// </summary>
    public interface IRules
    {
        int FieldRows { get; }
        int FieldColumns { get; }
        int PlayerCardsAmount { get; }
        int CurrencyAmount { get; }
    }
}