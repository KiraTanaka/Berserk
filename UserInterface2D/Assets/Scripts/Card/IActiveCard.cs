using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IActiveCard
{
    string InstId { get; set; }
    GameObject GameObject { get; }
    void ChangeHealth(int health);
    void Close();
    void Open();
    void SetClose(bool value);
    bool IsClosed();
}

