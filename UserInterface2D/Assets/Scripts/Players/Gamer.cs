using Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Gamer : MonoBehaviour, IPlayer 
{
    public GameObject CardPrefab;
    public GameObject HeroPrefab;
    public GameObject ActiveCardPrefab;
    public GameObject CoinPrefab;
    Vector3 _positionHero = new Vector3(-37.506f, -105.735f, 0);
    List<Vector3> _positionsCards = new List<Vector3>() {
    new Vector3(-2.51f, -5.5f,0),
    new Vector3(-1.2f, -5.5f, 0),
    new Vector3(-0.04f, -5.5f, 0),
    new Vector3(1.12f, -5.5f, 0)
    };    
    string pathToCreaturePositionGamer = @"Assets\Scripts\PositionOfActiveCardsPlayer.txt";
    string pathToCoinPositionGamer = @"Assets\Scripts\PositionsOfCoinsPlayer.txt";
    PlayerUnity _playerUnity = new PlayerUnity();
    public Player player
    {
        get { return _playerUnity.player; }
        set
        {
            _playerUnity.player = value;
            _playerUnity.Initialization(_positionsCards,_positionHero, pathToCreaturePositionGamer,pathToCoinPositionGamer);
            _playerUnity.SetPrefab(CardPrefab,HeroPrefab,ActiveCardPrefab, CoinPrefab);
        }
    }
    public void LocateCards()
    {
        _playerUnity.LocateCards();
    }
}

