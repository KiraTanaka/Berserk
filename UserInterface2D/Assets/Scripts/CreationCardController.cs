using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Domain.Cards;
using Domain.Process;

public class CreationCardController: MonoBehaviour
{
    Transform parentCard;
    public GameObject BorderCard;
    public GameObject BorderActiveCard;
    CoordinateParser parser = new CoordinateParser();
    Dictionary<Vector3, bool> positionsActiveCards = new Dictionary<Vector3, bool>();
    Vector2 scaleActiveCard = new Vector2(20f, 20f);
    Vector2 scaleHero = new Vector2(31.92f, 31.92f);
    public void Setting()
    {
        BorderCard = GameObject.FindWithTag("BorderCard");
        BorderActiveCard = GameObject.FindWithTag("BorderActiveCard");
        parentCard = GameObject.FindWithTag("Canvas").transform;    
    }
    public void LoadPositionsActiveCards(string name)
    {
        parser.Parse(name).ForEach(x => positionsActiveCards.Add(x, false));
    }
    public GameObject CreateCardInHand(GameObject prefab, Card card, Vector3 position, int sortingOrder)
    {
        GameObject sprite = CreateCard(prefab,card,position);
        sprite.GetComponent<CardInHand>().SetCard(card);

        LoadSprite(sprite, card.Id.ToString() + "_origin", sortingOrder);
        sprite.GetComponent<SelectionCreature>().border = BorderCard;
        return sprite;
    }
    public GameObject CreateSpriteHero(GameObject prefab, Card card, Vector3 position, int sortingOrder)
    {
        GameObject sprite = CreateCard(prefab, card, position);
        sprite.GetComponent<Hero>().SetCard(card);

        LoadSprite(sprite, card.Id.ToString(), sortingOrder);
        SetParent(sprite,scaleHero);
        return sprite;
    }
    public GameObject CreateActiveCard(GameObject prefab, Card card, int sortingOrder)
    {
        Vector3 position = positionsActiveCards.FirstOrDefault(x => !x.Value).Key;

        GameObject sprite = CreateCard(prefab, card, position);
        sprite.GetComponent<CardUnity>().SetCard(card);

        LoadSprite(sprite, card.Id.ToString(), sortingOrder);
        SetParent(sprite, scaleActiveCard);

        sprite.GetComponent<SelectionCreature>().border = BorderActiveCard;
        BorderCard.SetActive(false);
        
        positionsActiveCards[position] = true;
        return sprite;
    }
    GameObject CreateCard(GameObject prefab, Card card, Vector3 position)
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

