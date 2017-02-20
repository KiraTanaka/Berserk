using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Infrastructure;
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

        private List<Vector3> _positionsCards;


        private readonly CreationCardController _cardsController = new CreationCardController();

        private readonly CreateCoinsController _coinsController = new CreateCoinsController();
        
        private readonly List<GameObject> _cardsInHand = new List<GameObject>();

        private readonly List<GameObject> _cardsInGame = new List<GameObject>();
        
        private readonly List<GameObject> _coins = new List<GameObject>();

        
        public void Initialize(string playerId, List<Vector3> positionsCards, Vector3 positionHero, string namePlayer)
        {
            Id = new Guid(playerId);
            SetClient();   
            ControllerSettings(namePlayer);
            SetPositionsCards(positionsCards,positionHero);        
        }

        public void ClientSettings() => _client.Settings(Id.ToString());

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

        public void SetPositionsCards(List<Vector3> positionsCards, Vector3 positionHero)
        {
            _positionsCards = positionsCards;
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
            for (int i = 0; i < cardsInfo.Length; i++)
            {
                GameObject sprite = _cardsController.CreateCardInHand(
                    CardPrefab, cardsInfo[i], _positionsCards[i], i,Id.ToString());

                _client.SubscribeToSelectCardInHand(sprite);
                _cardsInHand.Add(sprite);           
            }
        }

        public void CreateSpriteHero(GameObject prefab, CardInfo heroInfo, Vector3 position, int sortingOrder)
        {
            GameObject sprite = _cardsController.CreateSpriteHero(
                prefab, heroInfo, position, sortingOrder,Id.ToString());

            _client.SubscribeToEventsHero(sprite);
            _hero = sprite;
        }

        public void CreateActiveCard(CardInfo cardInfo, string playerId)
        {
            if (playerId != Id.ToString()) return;

            GameObject sprite = _cardsController.CreateActiveCard(
                ActiveCardPrefab, cardInfo, 1, Id.ToString());

            _client.SubscribeToEventsActiveCard(sprite);
            _cardsInGame.Add(sprite);
        }

        public void OnAddCoin()
        {
            _coinsController.CreateCoin(CoinPrefab, Id.ToString());
        } 
        
        public void DestroyCardInHand(string instId, string playerId)
        {
            if (playerId != Id.ToString()) return;
            if (_cardsInHand.Count == 0) return;

            var card = _cardsInHand
                .Select(x => x.GetComponent<CardInHand>())
                .FirstOrDefault(x => x.InstId == instId);

            if(card == null) return;

            _cardsInHand.Remove(card.gameObject);
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
            if (health <= 0) _cardsInGame.Remove(card.GameObject);
        }

        public void OnChangeClosed(string instId, bool closed)
        {
            var card = SetClosed(_cardsInGame, instId, closed);
            card = card ?? SetClosed(new List<GameObject> { _hero }, instId, closed);
            // TODO а куда карта идет дальше?
        }

        private static IActiveCard SetHealth(List<GameObject> cards, string instId, int value)
        {
            var script = cards
                .Select(x => x.GetComponent<IActiveCard>())
                .FirstOrDefault(x => x.InstId == instId);
            script?.ChangeHealth(value);
            return script;
        }

        private static IActiveCard SetClosed(List<GameObject> cards, string instId, bool value)
        {
            var script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.InstId == instId);
            script?.SetClose(value);
            return script;
        }

        public void OpenAll(string playerId)
        {
            if (playerId != Id.ToString()) return;
            _cardsInGame.Select(x => x.GetComponent<CardUnity>()).ToList().ForEach(x => x.SetClose(false));
            _hero.GetComponent<Hero>().SetClose(false);
            _coins.Select(x => x.GetComponent<CoinUnity>()).ToList().ForEach(x => x.Open());
        }

        public void UpdateCountCoins(string[] playersId, int[] countCoinsPlayers)
        {
            for (int i = 0; i < playersId.Length; i++)
            {
                if (playersId[i] == Id.ToString())
                    CreateCoins(countCoinsPlayers[i] - _coins.Count);
            }
        }
    }
}
