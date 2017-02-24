using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI.Controllers;
using Assets.Scripts.UI.Cards;
using Assets.Scripts.UI.Coins;
using Domain.GameProcess;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Players
{
    public class PlayerUnity : NetworkBehaviour
    {
        public GameObject CardPrefab;

        public GameObject HeroPrefab;

        public GameObject ActiveCardPrefab;

        public GameObject CoinPrefab;


        public Guid Id { get; set; }

        public Player Player { get; set; }


        private GameObject _hero;

        private ClientController _client;

        private Vector3 _positionHero;


        private readonly CreationCardController _cardsController = new CreationCardController();

        private readonly CreateCoinsController _coinsController = new CreateCoinsController();
        
        private readonly List<GameObject> _cardsInHand = new List<GameObject>();

        private readonly List<GameObject> _cardsInGame = new List<GameObject>();
        
        private readonly List<GameObject> _coins = new List<GameObject>();

        
        public void Initialize(string playerId, Vector3 positionHero, string namePlayer)
        {
            Id = new Guid(playerId);
            SetClient();   
            ControllerSettings(namePlayer);
            SetPositionHero(positionHero);        
        }

        public void ControllerSettings(string namePlayer)
        {
            _cardsController.LoadSetting(namePlayer);
            _coinsController.LoadPositionsCoins(namePlayer);
        }

        public void SetClient()
        {
            _client = GameObject
                .FindGameObjectsWithTag("Gamer")
                .Select(x => x.GetComponent<ClientController>())
                .FirstOrDefault(x => x.isLocalPlayer);
        }

        public void SetPositionHero(Vector3 positionHero)
        {
            _positionHero = positionHero;
        }

        public void SetPrefab(GameObject cardPrefab, GameObject heroPrefab, GameObject activeCardPrefab, GameObject coinPrefab)
        {
            CardPrefab = cardPrefab;
            HeroPrefab = heroPrefab;
            ActiveCardPrefab = activeCardPrefab;
            CoinPrefab = coinPrefab;
        }

        public void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
        {
            CreateSpriteHero(HeroPrefab, heroInfo, _positionHero, 1);
            CreateCardsInHand(cardsInfo);
            CreateCoins(countCoin);
        }

        public  void CreateCoins(int countCoin)
        {
            for (int i = 0; i < countCoin; i++)
            {
                _coins.Add(_coinsController.CreateCoin(CoinPrefab, Id.ToString()));
            }
        }

        public void CreateCardsInHand( CardInfo[] cardsInfo)
        {
            cardsInfo.ToList().ForEach(x => CreateCardInHand(x));
        }

        public void CreateCardInHand(CardInfo cardInfo)
        {
            GameObject sprite = _cardsController.CreateCardInHand(CardPrefab, cardInfo);

            _client.SubscribeToSelectCardInHand(sprite);
            _cardsInHand.Add(sprite);
        }

        public void CreateSpriteHero(GameObject prefab, CardInfo heroInfo, Vector3 position, int sortingOrder)
        {
            GameObject sprite = _cardsController.CreateHero(prefab, heroInfo, position);

            _client.SubscribeToEventsActiveCard(sprite);
            _hero = sprite;
        }

        public void CreateActiveCard(CardInfo cardInfo, string playerId)
        {
            if (playerId != Id.ToString()) return;

            GameObject sprite = _cardsController.CreateEntity(ActiveCardPrefab, cardInfo);

            _client.SubscribeToEventsActiveCard(sprite);
            _cardsInGame.Add(sprite);
        }

        public void DestroyCardInHand(string instId, string playerId)
        {
            if (playerId != Id.ToString() || _cardsInHand.Count == 0) return;

            var card = _cardsInHand.Select(x => x.GetComponent<CardInHand>()).FirstOrDefault(x => x.InstId == instId);

            if(card == null) return;

            _cardsInHand.Remove(card.gameObject);
            card.SetOriginalPosition();
            _cardsController.ClearPositionCardInHand(card);
            card.DestroyCard();
        }

        public void CloseCoins(int count, string playerId)
        {
            if (playerId != Id.ToString()) return;
            _coins.Select(x => x.GetComponent<CoinUnity>()).Where(x => !x.IsClosed).Take(count).ToList().ForEach(x => x.Close());
        }

        public void OnChangeHealth(string instId, int health)
        {
            var card = SetHealth(_cardsInGame, instId, health);
            card = card ?? SetHealth(new List<GameObject>() { _hero }, instId, health);                     
        }

        public void OnChangeClosed(string instId, bool closed)
        {
            var card = SetClosed(_cardsInGame, instId, closed);
            card = card ?? SetClosed(new List<GameObject> { _hero }, instId, closed);
            // TODO а куда карта идет дальше?
        }

        private IActiveCard SetHealth(List<GameObject> cards, string instId, int value)
        {
            var script = cards
                .Select(x => x.GetComponent<IActiveCard>())
                .FirstOrDefault(x => x.InstId == instId);
            script?.ChangeHealth(value);
            if (value > 0 || script==null) return script;
            cards.Remove(script.GameObject);
            script.DestroyCard();
            return script;
        }

        private IActiveCard SetClosed(List<GameObject> cards, string instId, bool value)
        {
            var script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.InstId == instId);
            if (value) script?.Close();
            else script?.Open();
            return script;
        }

        public void OpenAll(string playerId)
        {
            if (playerId != Id.ToString()) return;
            _coins.Select(x => x.GetComponent<CoinUnity>()).ToList().ForEach(x => x.Open());
        }

        public void UpdateCountCoins(string playerId, int countCoinsPlayer)
        {
            if (playerId != Id.ToString()) return;
            CreateCoins(countCoinsPlayer - _coins.Count);
        }
        public void UpdateCardsInHand(string playerId, CardInfo[] cardsInfo)
        {
            if (playerId != Id.ToString()) return;

            cardsInfo.ToList().ForEach(x =>
            {
                if (_cardsInHand.Select(card
                    => card.GetComponent<CardInHand>()).FirstOrDefault(card => card.InstId == x.InstId) == null)
                    CreateCardInHand(x);
            });
        }
    }
}
