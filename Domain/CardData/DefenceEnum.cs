namespace Domain.CardData
{
    /// <summary>
    /// Выбор опции защиты персонажа.
    /// </summary>
    public enum DefenceEnum
    {
        /// <summary>
        /// Блок не удался.
        /// </summary>
        Fail,

        /// <summary>
        /// Блок удался.
        /// </summary>
        Block,

        /// <summary>
        /// Блок удался + контратака.
        /// </summary>
        Counterattack
    }
}
