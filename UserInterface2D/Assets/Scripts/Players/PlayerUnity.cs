using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Domain.Cards;
using Domain.Process;
using System;
using System.Linq;

public class PlayerUnity : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject HeroPrefab;
    public GameObject ActiveCardPrefab;
    public GameObject CoinPrefab;
    private GameScript _game;
    private Vector3 _positionHero;
    private List<Vector3> _positionsCards;
    private CreationCardController _cardsController = new CreationCardController();
    private CreateCoinsController _coinsController = new CreateCoinsController();
    public Player player { get; set; }
    public void Initialization(List<Vector3> positionsCards, Vector3 positionHero, 
        string pathToCreaturePosition, string pathToCoinPosition)
    {
        _cardsController.Setting();
        SetPositionsCards(positionsCards,positionHero);
        LoadPositions(pathToCreaturePosition, pathToCoinPosition);
        _game = GameObject.FindWithTag("Scripts").GetComponent<GameScript>();
        player.Money.onAddCoin += onAddCoin;
    }
    private void LoadPositions(string pathToCreaturePosition, string pathToCoinPosition)
    {
        _cardsController.LoadPositionsActiveCards(pathToCreaturePosition);
        _coinsController.LoadPositionsCoins(pathToCoinPosition);
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
    public void LocateCards()
    {
        Card[] activeCards = player.ActiveDeck;
        CreateSpriteHero(HeroPrefab, player.Hero, _positionHero, 1);
        
        for (int i = 0; i < activeCards.Length; i++)
        {
            CreateCardInHand(CardPrefab, activeCards[i], _positionsCards[i],i);
        }
        player.Money.ForEach(coin=>_coinsController.CreateCoin(CoinPrefab,coin));
    }
    void CreateCardInHand(GameObject prefab, Card card, Vector3 position, int sortingOrder)
    {
        GameObject sprite =  _cardsController.CreateCardInHand(CardPrefab, card, position, sortingOrder);
        sprite.GetComponent<CardInHand>().onSelectCard += onSelectCard;
    }
    void CreateSpriteHero(GameObject prefab, Card card, Vector3 position, int sortingOrder)
    {
        GameObject sprite = _cardsController.CreateSpriteHero(prefab,card,position, sortingOrder);
        sprite.GetComponent<Hero>().onSelectCard += onSelectActiveCard;
    }
    bool onSelectCard(Card card)
    {
        bool result = player.Hire(card.Id);//изменить логику в hire
        if (result) CreateActiveCard(card);
        return result;
    }
    void CreateActiveCard(Card card)
    {
        GameObject sprite = _cardsController.CreateActiveCard(ActiveCardPrefab, card, 1);
        SubscribeToEvents(sprite);
    }
    void SubscribeToEvents(GameObject sprite)
    {
        CardUnity script = sprite.GetComponent<CardUnity>();
        script.onSelectCard += onSelectActiveCard;
        _game.onAfterMove += script.onAfterMove;
    }
    void onSelectActiveCard(Card card)
    {
        GameState state = _game.GetGameState();
        if (player.Id == state.MovingPlayer.Id && _game.ActionCard == null && card.Type!=CardTypeEnum.Hero)//пока так
            _game.ActionCard = card;
        else if (_game.ActionCard != card && _game.ActionCard != null)
            _game.TargetCard = card;
        else
            _game.SetToZeroCards();
    }
    void onAddCoin(Coin coin) => _coinsController.CreateCoin(CoinPrefab, coin); 
}
