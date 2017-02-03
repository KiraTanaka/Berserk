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
    public override void OnStartLocalPlayer()
    {
        if (!isLocalPlayer) return;
        CmdConnectPlayer();
    }
    [Command]
    void CmdConnectPlayer()
    {
        if (!isServer) return;

        Player player = GameObject.FindWithTag("Scripts").GetComponent<GameScript>().GetPlayer();
        CardInfo heroInfo = new CardInfo() { _id = player.Hero.Id, _health = player.Hero.Health };
        RpcInitialization(player.Id.ToString(),heroInfo, player.ActiveDeck.Select(x => x.Id).ToArray(), player.Money.Count);
    }
    [ClientRpc]
    void RpcInitialization(string playerId, CardInfo heroInfo, int[] cardsId, int countCoin)
    {
        if (!isLocalPlayer) return;

        _playerUnity.Initialization(playerId,_positionsCards, _positionHero, gameObject.tag);
        _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab, CoinPrefab);
        LocateCards(heroInfo, cardsId, countCoin);
    }
    public void LocateCards(CardInfo heroInfo, int[] cardsId, int countCoin)
    {
        _playerUnity.LocateCards(heroInfo, cardsId, countCoin);
    }
    public void CreateActiveCard(CardInfo cardInfo, string playerId)
    {
        if (_playerUnity.Id.ToString() == playerId)
            _playerUnity.CreateActiveCard(cardInfo);
    }
}

