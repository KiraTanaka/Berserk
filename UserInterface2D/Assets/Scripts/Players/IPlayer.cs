using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;
using Domain.Process;

interface IPlayer
{
    Player player { get; set; }
    void LocateCards();
}

