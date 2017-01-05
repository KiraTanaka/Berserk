using System;
using System.Collections.Generic;

namespace Domain.CardData
{
    /// <summary>
    /// Действие карты. Определяется в момент реализации в плагине.
    /// </summary>
    public class CardAction
    {
        public string Description { get; set; }

        /// <summary>
        /// Определяет поведение карты при простой атаке.
        /// Возращает true, в случае отсутствия ошибок при выполнении.
        /// Успешность проведения атаки не гарантируется.
        /// </summary>
        public Func<ICard, ICard, AttackEnum, DefenceEnum, bool> SimpleAttack { get; set; }

        /// <summary>
        /// Определяет поведение карты при использовании способности.
        /// Возращает true, в случае отсутствия ошибок при выполнении.
        /// Успешность проведения атаки не гарантируется.
        /// </summary>
        public Func<ICard, IEnumerable<ICard>, AttackEnum, DefenceEnum, bool> FeatureAttack { get; set; }
    }
}