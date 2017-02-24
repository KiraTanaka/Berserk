﻿using System.Collections.Generic;
using Assets.Scripts.UI.Cards;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Players
{
    public class Enemy : NetworkBehaviour, IPlayer
    {
        public GameObject CardPrefab;

        public GameObject HeroPrefab;

        public GameObject ActiveCardPrefab;

        public GameObject CoinPrefab;


        private readonly Vector3 _positionHero = new Vector3(-40.698f, 127.68f, 0);

        private readonly PlayerUnity _playerUnity = new PlayerUnity();


        public string GetId() => _playerUnity.Id.ToString();
        
        public void OnStartPlayer(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
        {
            _playerUnity.Initialize(playerId, _positionHero, gameObject.tag);
            _playerUnity.SetPrefab(CardPrefab, HeroPrefab, ActiveCardPrefab, CoinPrefab);
            _playerUnity.CreateSpriteHero(HeroPrefab, heroInfo, _positionHero, 1);
            _playerUnity.CreateCoins(countCoin);
        }

        public void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
            => _playerUnity.LocateCards(heroInfo, cardsInfo, countCoin);

        public void CreateActiveCard(CardInfo cardInfo, string playerId)
            => _playerUnity.CreateActiveCard(cardInfo, playerId);

        public void DestroyCardInHand(string instId, string playerId)
            => _playerUnity.DestroyCardInHand(instId, playerId);

        public void CloseCoins(int count, string playerId)
            => _playerUnity.CloseCoins(count, playerId);

        public void OnChangeHealthCard(string instId, int health)
            => _playerUnity.OnChangeHealth(instId, health);

        public void OnChangeClosedCard(string instId, bool closed)
            => _playerUnity.OnChangeClosed(instId, closed);

        public void OpenAll(string playerId) 
            => _playerUnity.OpenAll(playerId);

        public void UpdateCountCoins(string playerId, int countCoinsPlayer) 
            => _playerUnity.UpdateCountCoins(playerId, countCoinsPlayer);
    }
}
