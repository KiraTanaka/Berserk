using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Process;

public class CoinUnity : MonoBehaviour {
    private Coin _coin;
    Quaternion closed = Quaternion.Euler(0, 0, 90);
    Quaternion open = Quaternion.Euler(0, 0, 0);
    public void SetCoin(Coin coin)
    {
        _coin = coin;
        _coin.onChangeClosed += onChangeClosed;
    }
    public void onChangeClosed()
    {
        transform.rotation = (_coin.Closed) ? closed : open;
    }
}
