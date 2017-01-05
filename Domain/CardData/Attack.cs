using System.Collections.Generic;

namespace Domain.CardData
{
    /// <summary>
    /// Содержит величины простой атаки персонажа.
    /// Величины: Слабая, Средняя, Сильная.
    /// </summary>
    public class Attack
    {
        // Реализовано через Dictionary для возможности выбора силы атаки
        // путем передачи опции, что необходимо в экшенах персонажа.
        private readonly Dictionary<AttackEnum, int> _values;

        public int this[AttackEnum option] => _values[option];

        public Attack(int low, int mid, int high)
        {
            _values = new Dictionary<AttackEnum, int>
            {
                [AttackEnum.Low] = low,
                [AttackEnum.Mid] = mid,
                [AttackEnum.High] = high
            };
        }

        public override string ToString()
            => $"{nameof(Attack)}: Low={_values[AttackEnum.Low]}, " +
               $"Mid={_values[AttackEnum.Mid]}, High={_values[AttackEnum.High]}";
    }
}
