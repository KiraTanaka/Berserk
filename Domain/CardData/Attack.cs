using System.Collections.Generic;

namespace Domain.CardData
{
    public class Attack
    {
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
    }
}
