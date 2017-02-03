using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Domain.Cards;
using Domain.Process;
using UnityEngine.Networking;

public class CreationCardController: MonoBehaviour
{
    Transform parentCard;
    public GameObject BorderCard;
    public GameObject BorderActiveCard;
    CoordinateParser parser = new CoordinateParser();
    Dictionary<Vector3, bool> positionsActiveCards = new Dictionary<Vector3, bool>();
    Vector2 scaleActiveCard = new Vector2(20f, 20f);
    Vector2 scaleHero = new Vector2(31.92f, 31.92f);
    public void Setting(string namePlayer)
    {
        BorderCard = GameObject.FindWithTag("BorderCard");
        BorderActiveCard = GameObject.FindWithTag("BorderActiveCard");
        parentCard = GameObject.FindWithTag("Canvas").transform;
        LoadPositionsActiveCards(namePlayer);
    }
    public void LoadPositionsActiveCards(string namePlayer)
         => parser.GetActiveCardsPositions(namePlayer).ForEach(x => positionsActiveCards.Add(x, false));
    public GameObject CreateCardInHand(GameObject prefab, int cardId, Vector3 position, int sortingOrder)
    {
        GameObject sprite = CreateCard(prefab,position);
        sprite.GetComponent<CardInHand>().SetCard(cardId);

        LoadSprite(sprite, cardId.ToString() + "_origin", sortingOrder);
        sprite.GetComponent<SelectionCreature>().border = BorderCard;
        return sprite;
    }
    public GameObject CreateSpriteHero(GameObject prefab, CardInfo heroInfo, Vector3 position, int sortingOrder, string playerId)
    {
        GameObject sprite = CreateCard(prefab, position);
        sprite.GetComponent<Hero>().SetCard(heroInfo);
        sprite.GetComponent<Hero>().PlayerId = playerId;

        LoadSprite(sprite, heroInfo._id.ToString(), sortingOrder);
        SetParent(sprite,scaleHero);
        return sprite;
    }
    public GameObject CreateActiveCard(GameObject prefab, CardInfo cardInfo, int sortingOrder, string playerId)
    {
        Vector3 position = positionsActiveCards.FirstOrDefault(x => !x.Value).Key;

        GameObject sprite = CreateCard(prefab, position);
        sprite.GetComponent<CardUnity>().SetCard(cardInfo);
        sprite.GetComponent<CardUnity>().PlayerId = playerId;

        LoadSprite(sprite, cardInfo._id.ToString(), sortingOrder);
        SetParent(sprite, scaleActiveCard);

        sprite.GetComponent<SelectionCreature>().border = BorderActiveCard;
        BorderCard.SetActive(false);
        
        positionsActiveCards[position] = true;
        return sprite;
    }
    GameObject CreateCard(GameObject prefab, Vector3 position)
    {
        GameObject sprite = Instantiate(prefab, position, Quaternion.identity);
        return sprite;
    }
    void LoadSprite(GameObject sprite, string name, int sortingOrder)
    {
        SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(name);
        renderer.sortingOrder = sortingOrder;
    }
    void SetParent(GameObject sprite, Vector2 localScale)
    {
        sprite.transform.localScale = localScale;
        sprite.transform.SetParent(parentCard, false);
    }
}

