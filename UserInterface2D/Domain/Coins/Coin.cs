namespace Domain.Coins
{
    public class Coin
    {
        public bool Closed { get; protected set; }

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
            OnChangeClosed?.Invoke();
        }

        #region delegates and events

        public delegate void OnChangeClosedHandler();

        public event OnChangeClosedHandler OnChangeClosed;

        #endregion
    }
}
