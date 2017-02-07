using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IActiveCard
{
    string InstId { get; set; }
    void ChangeHealth(int health);
    void Close();
    void Open();
    void SetClose(bool value);
    bool IsClosed();
}

