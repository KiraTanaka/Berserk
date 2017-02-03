using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using Domain.Process;
using System;
using System.Linq;
using UnityEngine.Networking;

public class PlayerUnity : NetworkBehaviour
{
    public Guid Id { get; set; }
    public GameObject CardPrefab;
    public GameObject HeroPrefab;
    public GameObject ActiveCardPrefab;
    public GameObject CoinPrefab;
    private GameScript _game;
    private Vector3 _positionHero;
    private List<Vector3> _positionsCards;
    private CreationCardController _cardsController = new CreationCardController();
    private CreateCoinsController _coinsController = new CreateCoinsController();
    private Client _client;
    public List<int> CardsId;
    public Player player { get; set; }
    public void Initialization(string playerId, List<Vector3> positionsCards, Vector3 positionHero, string namePlayer)
    {
        Id = new Guid(playerId);
        SetClient();
        _client.Settings(this);
        ControllerSettings(namePlayer);
        SetPositionsCards(positionsCards,positionHero);
        
    }
    public void ControllerSettings(string namePlayer)
    {
        _cardsController.Setting(namePlayer);
        _coinsController.LoadPositionsCoins(namePlayer);
    }
    public void SetClient() => _client = GameObject.FindGameObjectsWithTag("Gamer").Select(x => x.GetComponent<Client>())
            .FirstOrDefault(x => x.isLocalPlayer);   
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
    public void LocateCards(CardInfo heroInfo, int[] cardsId, int countCoin)
    {
        CardsId = cardsId.ToList();

        CreateSpriteHero(HeroPrefab, heroInfo, _positionHero, 1);
        CreateCardsInHand(CardPrefab, cardsId);
        CreateCoins(CoinPrefab, countCoin);
    }
    public  void CreateCoins(GameObject CoinPrefab,int countCoin)
    {
        for (int i = 0; i < countCoin; i++)
        {
            _coinsController.CreateCoin(CoinPrefab, Id.ToString());
        }
    }
    public void CreateCardsInHand(GameObject prefab, int[] cardsId)
    {
        for (int i = 0; i < cardsId.Length; i++)
        {
            GameObject sprite = _cardsController.CreateCardInHand(CardPrefab, cardsId[i], _positionsCards[i], i);
            _client.SubscribeToSelectCardInHand(sprite);           
        }
    }
    public void CreateSpriteHero(GameObject prefab, CardInfo heroInfo, Vector3 position, int sortingOrder)
    {
        GameObject sprite = _cardsController.CreateSpriteHero(prefab, heroInfo, position, sortingOrder,Id.ToString());
        _client.SubscribeToSelectHero(sprite);
    }
    public void CreateActiveCard(CardInfo cardInfo)
    {
        GameObject sprite = _cardsController.CreateActiveCard(ActiveCardPrefab, cardInfo, 1, Id.ToString());
        _client.SubscribeToActiveCard(sprite);
    }
    public void onAddCoin() => _coinsController.CreateCoin(CoinPrefab,Id.ToString()); 
}
