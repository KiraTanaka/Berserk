using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;
using Domain.Process;

public interface IPlayer
{
    PlayerUnity _playerUnity { get; set; }
    void LocateCards(int heroId, int heroHealth, int[] cardsId, int countCoin);
}

