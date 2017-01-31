using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Process
{
    public class Money
    {
        private List<Coin> _coins;
        #region delegates and events
        public delegate void OnAddCoinHandler(Coin coin);
        public event OnAddCoinHandler onAddCoin;
        #endregion
        public Money()
        {
            _coins = new List<Coin>();
        }
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
            => _coins.ForEach(x => x.Open());
        public void AddMoney(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var coin = new Coin();
                _coins.Add(coin);
                onAddCoin?.Invoke(coin);
            }
        }
        public void ForEach(Action<Coin> action)
            => _coins.ForEach(action);
    }
}
