using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using Domain.Process;

public class Enemy : MonoBehaviour,IPlayer {
    public GameObject CardPrefab;
    public GameObject HeroPrefab;
    public GameObject ActiveCardPrefab;
    Vector3 _positionHero = new Vector3(-40.698f, 127.68f, 0);
    List<Vector3> _positionsCards = new List<Vector3>() {
    new Vector3(-2.51f, 5.8f,0),
    new Vector3(-1.2f, 5.8f, 0),
    new Vector3(-0.04f, 5.8f, 0),
    new Vector3(1.12f, 5.8f, 0)
    };
    PlayerUnity _playerUnity = new PlayerUnity();
    string pathToCreaturePositionEnemy = @"Assets\Scripts\PositionOfActiveCardsEnemy.txt";
    public Player player
    {
        get { return _playerUnity.player; }
        set {
            _playerUnity.player = value;
            _playerUnity.Initialization(_positionsCards, _positionHero, pathToCreaturePositionEnemy);
            _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab);
        }
    }
    public void LocateCards()
    {
        _playerUnity.LocateCards();
    }
}
