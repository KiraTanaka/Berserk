using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using Domain.Process;
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
    List<GameObject> _cardsInHand = new List<GameObject>();
    List<GameObject> _cardsInGame = new List<GameObject>();
    GameObject _hero;
    List<GameObject> _coins = new List<GameObject>();
    public Player player { get; set; }
    public void Initialization(string playerId, List<Vector3> positionsCards, Vector3 positionHero, string namePlayer)
    {
        Id = new Guid(playerId);
        SetClient();   
        ControllerSettings(namePlayer);
        SetPositionsCards(positionsCards,positionHero);        
    }
    public void ClientSettings() => _client.Settings(Id.ToString());
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
    public void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin)
    {
        CreateSpriteHero(HeroPrefab, heroInfo, _positionHero, 1);
        CreateCardsInHand(CardPrefab, cardsInfo);
        CreateCoins(CoinPrefab, countCoin);
    }
    public  void CreateCoins(GameObject CoinPrefab,int countCoin)
    {
        for (int i = 0; i < countCoin; i++)
        {
            _coins.Add(_coinsController.CreateCoin(CoinPrefab, Id.ToString()));
        }
    }
    public void CreateCardsInHand(GameObject prefab, CardInfo[] cardsInfo)
    {
        for (int i = 0; i < cardsInfo.Length; i++)
        {
            GameObject sprite = _cardsController.CreateCardInHand(CardPrefab, cardsInfo[i], _positionsCards[i], i,Id.ToString());
            _client.SubscribeToSelectCardInHand(sprite);
            _cardsInHand.Add(sprite);           
        }
    }
    public void CreateSpriteHero(GameObject prefab, CardInfo heroInfo, Vector3 position, int sortingOrder)
    {
        GameObject sprite = _cardsController.CreateSpriteHero(prefab, heroInfo, position, sortingOrder,Id.ToString());
        _client.SubscribeToSelectHero(sprite);
        _hero = sprite;
    }
    public void CreateActiveCard(CardInfo cardInfo, string playerId)
    {
        if (playerId != Id.ToString()) return;
        GameObject sprite = _cardsController.CreateActiveCard(ActiveCardPrefab, cardInfo, 1, Id.ToString());
        _client.SubscribeToSelectActiveCard(sprite);
        _cardsInGame.Add(sprite);
    }
    public void onAddCoin() => _coinsController.CreateCoin(CoinPrefab,Id.ToString());
    public void DestroyCardInHand(string instId, string playerId)
    {
        if (playerId != Id.ToString()) return;
        _cardsInHand.Select(x => x.GetComponent<CardInHand>()).FirstOrDefault(x => x.InstId == instId)?.DestroyCard();
    }
    public void CloseCoins(int count, string playerId)
    {
        if (playerId != Id.ToString()) return;
        _coins.Select(x => x.GetComponent<CoinUnity>()).Where(x => !x.IsClosed()).Take(count).ToList().ForEach(x => x.Close());
    }
    public void OnChangeHealth(string instId, int health)
    {
        if (!SetHealth(_cardsInGame, instId, health))
            SetHealth(new List<GameObject>() { _hero }, instId, health);
    }
    public void OnChangeClosed(string instId, bool closed)
    {
        if (!SetClosed(_cardsInGame, instId, closed))
            SetClosed(new List<GameObject>() { _hero }, instId, closed);
    }
    bool SetHealth(List<GameObject> cards, string instId, int value)
    {
        IActiveCard script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.InstId == instId);
        script?.ChangeHealth(value);
        return (script == null) ? false : true;
    }
    bool SetClosed(List<GameObject> cards, string instId, bool value)
    {
        IActiveCard script = cards.Select(x => x.GetComponent<IActiveCard>()).FirstOrDefault(x => x.InstId == instId);
        script?.SetClose(value);
        return (script == null) ? false : true;
    }
    public void OpenAll(string playerId)
    {
        if (playerId != Id.ToString()) return;
        _cardsInGame.Select(x => x.GetComponent<CardUnity>()).ToList().ForEach(x => x.SetClose(false));
        _hero.GetComponent<Hero>().SetClose(false);
        _coins.Select(x => x.GetComponent<CoinUnity>()).ToList().ForEach(x => x.Open());
    }
}
