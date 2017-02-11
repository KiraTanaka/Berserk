using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Coins
{
    public class Money : IEnumerable<Coin>
    {
        private readonly List<Coin> _coins = new List<Coin>();

        public int Count => _coins.Count;

        /// <summary>
        /// Закрывает указанное количество монет.
        /// </summary>
        public static Money operator -(Money money, int count)
        {
            if (count > money.Count) return money;
            money._coins.Where(x=>!x.Closed).Take(count).ToList().ForEach(x=>x.Close());
            return money;
        }

        /// <summary>
        /// Добавляет указанное количество монет.
        /// </summary>
        public static Money operator +(Money money, int count)
        {
            money.AddMoney(count);
            return money;
        }

        /// <summary>
        /// Открывает все монеты.
        /// </summary>
        public void OpenAll()
        {
            _coins.ForEach(x => x.Open());
        }

        public void AddMoney(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var coin = new Coin();
                _coins.Add(coin);
                OnAddCoin?.Invoke();
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Coin> GetEnumerator()
        {
            foreach (var coin in _coins)
                yield return coin;
        }

        #region delegates and events

        public delegate void OnAddCoinHandler();

        public event OnAddCoinHandler OnAddCoin;

        #endregion
    }
}
