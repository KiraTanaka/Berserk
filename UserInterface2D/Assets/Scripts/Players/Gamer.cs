using Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Gamer : NetworkBehaviour
{   
    public GameObject CardPrefab;
    public GameObject HeroPrefab;
    public GameObject ActiveCardPrefab;
    public GameObject CoinPrefab;
    private Vector3 _positionHero = new Vector3(-37.506f, -105.735f, 0);
    private List<Vector3> _positionsCards = new List<Vector3>() {
                                                new Vector3(-2.51f, -5.5f,0),
                                                new Vector3(-1.2f, -5.5f, 0),
                                                new Vector3(-0.04f, -5.5f, 0),
                                                new Vector3(1.12f, -5.5f, 0)
                                                };    
    public PlayerUnity _playerUnity { get; set; } = new PlayerUnity();
    public string GetId() => _playerUnity.Id.ToString();

    public void Initialization(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
    {
        if (!isLocalPlayer) return;

        _playerUnity.Initialization(playerId,_positionsCards, _positionHero, gameObject.tag);
        _playerUnity.ClientSettings();
        _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab, CoinPrefab);
        LocateCards(heroInfo, cardsInfo, countCoin);
    }
    public void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin) 
        => _playerUnity.LocateCards(heroInfo, cardsInfo, countCoin);
    public void onAddCoin() => _playerUnity.onAddCoin();
    public void CreateActiveCard(CardInfo cardInfo, string playerId) => _playerUnity.CreateActiveCard(cardInfo, playerId);
    public void DestroyCardInHand(string instId, string playerId) => _playerUnity.DestroyCardInHand(instId, playerId);
    public void CloseCoins(int count, string playerId) => _playerUnity.CloseCoins(count, playerId);
    public void OnChangeHealth(string instId, int health) => _playerUnity.OnChangeHealth(instId, health);
    public void OnChangeClosed(string instId, bool closed) => _playerUnity.OnChangeClosed(instId, closed);
    public void OpenAll(string playerId) => _playerUnity.OpenAll(playerId);
    public void UpdateCountCoins(string[] playersId, int[] countCoinsPlayers)
        => _playerUnity.UpdateCountCoins(playersId, countCoinsPlayers);
}

