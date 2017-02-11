using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Coins
{
    public class CoinUnity : NetworkBehaviour
    {
        public string PlayerId { get; set; }

        public bool IsClosed { get; private set; }


        private readonly Quaternion _close = Quaternion.Euler(0, 0, 90);

        private readonly Quaternion _open = Quaternion.Euler(0, 0, 0);


        public void Close() => SetClose(true);

        public void Open() => SetClose(false);

        private void SetClose(bool closed)
        {
            transform.rotation = closed ? _close : _open;
            IsClosed = closed;
        }
    }
}
