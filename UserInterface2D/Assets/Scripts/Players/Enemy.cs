using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using Domain.Process;
using UnityEngine.Networking;
using System.Linq;
using System;

public class Enemy : NetworkBehaviour {
    public GameObject CardPrefab;
    public GameObject HeroPrefab;
    public GameObject ActiveCardPrefab;
    public GameObject CoinPrefab;
    Vector3 _positionHero = new Vector3(-40.698f, 127.68f, 0);
    List<Vector3> _positionsCards = new List<Vector3>() {
    new Vector3(-2.51f, 5.8f,0),
    new Vector3(-1.2f, 5.8f, 0),
    new Vector3(-0.04f, 5.8f, 0),
    new Vector3(1.12f, 5.8f, 0)
    };
    public PlayerUnity _playerUnity { get; set; } = new PlayerUnity();
    public string GetId() => _playerUnity.Id.ToString();
    public void OnStartPlayer(string playerId, CardInfo heroInfo, int countCards, int countCoin)
    {
        _playerUnity.Initialization(playerId, _positionsCards, _positionHero, gameObject.tag);
        _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab, CoinPrefab);
        _playerUnity.CreateSpriteHero(HeroPrefab, heroInfo, _positionHero, 1);
        _playerUnity.CreateCoins(CoinPrefab, countCoin);
    }
    /*public void Initialization(string playerId, List<Vector3> positionsCards, Vector3 positionHero)
    {
        _playerUnity.Id = new Guid(playerId);
        _playerUnity.SetClient();
        _playerUnity.ControllerSettings(gameObject.tag);
        _playerUnity.SetPositionsCards(positionsCards, positionHero);
    }*/
    public void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
    {
        _playerUnity.LocateCards(heroInfo, cardsInfo, countCoin);
    }
    public void onAddCoin()
    {
        _playerUnity.onAddCoin();
    }
    public void CreateActiveCard(CardInfo cardInfo, string playerId)
    {
        _playerUnity.CreateActiveCard(cardInfo, playerId);
    }
    public void DestroyCardInHand(string instId, string playerId)
    {
        _playerUnity.DestroyCardInHand(instId, playerId);
    }
    public void CloseCoins(int count, string playerId)
    {
        _playerUnity.CloseCoins(count, playerId);
    }
    public void OnChangeHealth(string instId, int health)
    {
        _playerUnity.OnChangeHealth(instId, health);
    }
    public void OnChangeClosed(string instId, bool closed)
    {
        _playerUnity.OnChangeClosed(instId, closed);
    }
    public void OpenAll(string playerId)
    {
        _playerUnity.OpenAll(playerId);
    }
}
