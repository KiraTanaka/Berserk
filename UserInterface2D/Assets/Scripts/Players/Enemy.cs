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
    public void OnStartPlayer(string playerId, CardInfo heroInfo, int countCards, int countCoin)
    {
        Initialization(playerId, _positionsCards, _positionHero);
        _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab, CoinPrefab);
        _playerUnity.CreateSpriteHero(HeroPrefab, heroInfo, _positionHero, 1);
        //_playerUnity.CreateCardsInHand(CardPrefab, cardsId);
        _playerUnity.CreateCoins(CoinPrefab, countCoin);
    }
    public void Initialization(string playerId, List<Vector3> positionsCards, Vector3 positionHero)
    {
        _playerUnity.Id = new Guid(playerId);
        _playerUnity.SetClient();
        _playerUnity.ControllerSettings(gameObject.tag);
        _playerUnity.SetPositionsCards(positionsCards, positionHero);
    }
    [Command]
    void CmdConnectPlayer()
    {
        if (!isServer) return;

        Player player = GameObject.FindWithTag("Scripts").GetComponent<GameScript>().GetPlayer();
        CardInfo heroInfo = new CardInfo() { _id = player.Hero.Id, _health = player.Hero.Health };
        RpcInitialization(player.Id.ToString(), heroInfo, player.ActiveDeck.Select(x => x.Id).ToArray(), player.Money.Count);
    }
    [ClientRpc]
    void RpcInitialization(string playerId, CardInfo heroInfo, int[] cardsId, int countCoin)
    {
        if (!isLocalPlayer) return;

        _playerUnity.Initialization(playerId, _positionsCards, _positionHero, gameObject.tag);
        _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab, CoinPrefab);
        LocateCards(heroInfo, cardsId, countCoin);
    }
    public void LocateCards(CardInfo heroInfo, int[] cardsId, int countCoin)
    {
        _playerUnity.LocateCards(heroInfo, cardsId, countCoin);
    }
    public void onAddCoin()
    {
        _playerUnity.onAddCoin();
    }
    public void CreateActiveCard(CardInfo cardInfo, string playerId)
    {
        if (_playerUnity.Id.ToString()== playerId)
            _playerUnity.CreateActiveCard(cardInfo);
    }
}
