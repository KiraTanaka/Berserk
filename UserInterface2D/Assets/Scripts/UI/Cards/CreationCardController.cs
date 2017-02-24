using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts.Infrastructure.Coordinates;
using Assets.Scripts.UI.Transformations;
using UnityEngine;

namespace Assets.Scripts.UI.Cards
{
    public class CreationCardController: MonoBehaviour
    {
        private GameObject _borderCard;

        private GameObject _borderActiveCard;

        private Transform _parentCard;

        private readonly CoordinateParser _parser = new CoordinateParser();

        private readonly Dictionary<Vector3, bool> _positionsActiveCards = 
            new Dictionary<Vector3, bool>();

        private readonly Dictionary<Vector3, bool> _positionsCardsInHand =
            new Dictionary<Vector3, bool>();

        private readonly Vector2 _scaleActiveCard = new Vector2(20f, 20f);

        private readonly Vector2 _scaleHero = new Vector2(31.92f, 31.92f);


        public void LoadSetting(string namePlayer)
        {
            _borderCard = GameObject.FindWithTag("BorderCard");
            _borderActiveCard = GameObject.FindWithTag("BorderActiveCard");

            _parentCard = GameObject.FindWithTag("Canvas").transform;
            LoadPositionsActiveCards(namePlayer);
            LoadPositionsCardsInHand(namePlayer);
        }

        private void LoadPositionsActiveCards(string namePlayer)
        {
            _parser.GetActiveCardsPositions(namePlayer)
                .ForEach(x => _positionsActiveCards.Add(x, false));
        }

        private void LoadPositionsCardsInHand(string namePlayer)
        {
            _parser.GetCardsInHandPositions(namePlayer)
                .ForEach(x => _positionsCardsInHand.Add(x, false));
        }

        public GameObject CreateCardInHand(GameObject prefab, CardInfo cardInfo, 
             string playerId)
        {
            Vector3 position = _positionsCardsInHand.FirstOrDefault(x => !x.Value).Key;

            GameObject sprite = CreateCard(prefab,position);
            sprite.GetComponent<CardInHand>().SetCard(cardInfo.InstId);
            sprite.GetComponent<CardInHand>().PlayerId = playerId;

            LoadSprite(sprite, cardInfo.CardId.ToString() + "_origin", _positionsCardsInHand.Keys.ToList().IndexOf(position));
            sprite.GetComponent<SelectionController>().Border = _borderCard;
            _positionsCardsInHand[position] = true;
            return sprite;
        }

        public GameObject CreateSpriteHero(GameObject prefab, CardInfo heroInfo, 
            Vector3 position, int sortingOrder, string playerId)
        {
            GameObject sprite = CreateCard(prefab, position);
            sprite.GetComponent<Hero>().SetCard(heroInfo);
            sprite.GetComponent<Hero>().PlayerId = playerId;

            LoadSprite(sprite, heroInfo.CardId.ToString(), sortingOrder);
            SetParent(sprite,_scaleHero);
            return sprite;
        }

        public GameObject CreateActiveCard(GameObject prefab, CardInfo cardInfo, 
            int sortingOrder, string playerId)
        {
            Vector3 position = _positionsActiveCards.FirstOrDefault(x => !x.Value).Key;

            GameObject sprite = CreateCard(prefab, position);
            sprite.GetComponent<CardUnity>().SetCard(cardInfo);
            sprite.GetComponent<CardUnity>().PlayerId = playerId;

            LoadSprite(sprite, cardInfo.CardId.ToString(), sortingOrder);
            SetParent(sprite, _scaleActiveCard);

            sprite.GetComponent<SelectionController>().Border = _borderActiveCard;
            _borderCard.SetActive(false);
        
            _positionsActiveCards[position] = true;
            return sprite;
        }

        private static GameObject CreateCard(GameObject prefab, Vector3 position)
        {
            GameObject sprite = Instantiate(prefab, position, Quaternion.identity);
            return sprite;
        }

        private static void LoadSprite(GameObject sprite, string name, int sortingOrder)
        {
            SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>(name);
            renderer.sortingOrder = sortingOrder;
        }

        private void SetParent(GameObject sprite, Vector2 localScale)
        {
            sprite.transform.localScale = localScale;
            sprite.transform.SetParent(_parentCard, false);
        }
    }
}

