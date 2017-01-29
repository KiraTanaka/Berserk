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
    GameScript game;
    Vector3 _positionHero;
    List<Vector3> _positionsCards;
    CreationCardController cardController = new CreationCardController();
    public Player player { get; set; }
    public void Initialization(List<Vector3> positionsCards, Vector3 positionHero, string pathToCreaturePosition)
    {
        cardController.Setting();
        SetPositionsCards(positionsCards,positionHero);
        cardController.LoadPositionsActiveCards(pathToCreaturePosition);
        game = GameObject.FindWithTag("Scripts").GetComponent<GameScript>(); 
    }
    public void SetPositionsCards(List<Vector3> positionsCards, Vector3 positionHero)
    {
        _positionsCards = positionsCards;
        _positionHero = positionHero;
    }
    public void SetPrefab(GameObject cardPrefab, GameObject heroPrefab, GameObject activeCardPrefab)
    {
        CardPrefab = cardPrefab;
        HeroPrefab = heroPrefab;
        ActiveCardPrefab = activeCardPrefab;
    }
    public void LocateCards()
    {
        Card[] activeCards = player.ActiveDeck;
        CreateSpriteHero(HeroPrefab, player.Hero, _positionHero, 1);
        
        for (int i = 0; i < activeCards.Length; i++)
        {
            CreateCardInHand(CardPrefab, activeCards[i], _positionsCards[i],i);
        }
    }
    void CreateCardInHand(GameObject prefab, Card card, Vector3 position, int sortingOrder)
    {
        GameObject sprite =  cardController.CreateCardInHand(CardPrefab, card, position, sortingOrder);
        sprite.GetComponent<CardInHand>().onSelectCard += onSelectCard;
    }
    void CreateSpriteHero(GameObject prefab, Card card, Vector3 position, int sortingOrder)
    {
        GameObject sprite = cardController.CreateSpriteHero(prefab,card,position, sortingOrder);
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
        GameObject sprite = cardController.CreateActiveCard(ActiveCardPrefab, card, 1);
        SubscribeToEvents(sprite);
    }
    void SubscribeToEvents(GameObject sprite)
    {
        CardUnity script = sprite.GetComponent<CardUnity>();
        script.onSelectCard += onSelectActiveCard;
        game.onAfterMove += script.onAfterMove;
    }
    void onSelectActiveCard(Card card)
    {
        GameState state = game.GetGameState();
        if (player.Id == state.MovingPlayer.Id && game.ActionCard == null && card.Type!=CardTypeEnum.Hero)//пока так
            game.ActionCard = card;
        else if (game.ActionCard != card && game.ActionCard != null)
            game.TargetCard = card;
        else
            game.SetToZeroCards();
    }
}
