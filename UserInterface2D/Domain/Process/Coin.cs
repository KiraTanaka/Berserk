using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Process
{
    public class Coin
    {
        public bool Closed { get; protected set; } = false;
        #region delegates and events
        public delegate void OnChangeClosedHandler();
        public event OnChangeClosedHandler onChangeClosed;
        #endregion
        public void Open()
        {
            SetClose(false);
        }

        public void Close()
        {
            SetClose(true);
        }

        private void SetClose(bool value)
        {
            Closed = value;
            onChangeClosed?.Invoke();
        }
    }
}
