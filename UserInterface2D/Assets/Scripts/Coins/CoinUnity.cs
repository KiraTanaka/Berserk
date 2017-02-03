using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Process;
using UnityEngine.Networking;

public class CoinUnity : NetworkBehaviour
{
    public string PlayerId { get; set; }
    public bool _closed = false;
    Quaternion close = Quaternion.Euler(0, 0, 90);
    Quaternion open = Quaternion.Euler(0, 0, 0);
    public void Close() => SetClose(true);
    public void Open() => SetClose(false);
    private void SetClose(bool closed)
    {
        transform.rotation = (closed) ? close : open;
        _closed = closed;
    }
    public bool IsClosed()
    {
        return _closed;
    }
}
