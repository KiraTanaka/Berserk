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
        private Transform _parentCard;

        private readonly Dictionary<BorderEnum, GameObject> borders =
            new Dictionary<BorderEnum, GameObject>();

        private readonly CoordinateParser _parser = new CoordinateParser();

        private readonly Dictionary<Vector3, bool> _positionsActiveCards = 
            new Dictionary<Vector3, bool>();

        private readonly Dictionary<Vector3, bool> _positionsCardsInHand =
            new Dictionary<Vector3, bool>();

        private readonly Vector2 _scaleActiveCard = new Vector2(20f, 20f);

        private readonly Vector2 _scaleHero = new Vector2(31.92f, 31.92f);

        private readonly int sortingOrderActiveCard = 1;

        public void LoadSetting(string namePlayer)
        {
            borders.Add(BorderEnum.BorderCard, GameObject.FindWithTag(BorderEnum.BorderCard.ToString()));
            borders.Add(BorderEnum.BorderHero, GameObject.FindWithTag(BorderEnum.BorderHero.ToString()));
            borders.Add(BorderEnum.BorderActiveCard, GameObject.FindWithTag(BorderEnum.BorderActiveCard.ToString()));

            _parentCard = GameObject.FindWithTag("Canvas").transform;
            LoadPositionsActiveCards(namePlayer);
            LoadPositionsCardsInHand(namePlayer);
        }

        private void LoadPositionsActiveCards(string namePlayer)
            => _parser.GetActiveCardsPositions(namePlayer).ForEach(x => _positionsActiveCards.Add(x, false));

        private void LoadPositionsCardsInHand(string namePlayer)
            =>_parser.GetCardsInHandPositions(namePlayer) .ForEach(x => _positionsCardsInHand.Add(x, false));

        public GameObject CreateCardInHand(GameObject prefab, CardInfo cardInfo)
        {
            Vector3 position = _positionsCardsInHand.FirstOrDefault(x => !x.Value).Key;

            GameObject sprite = CreateCard(prefab, cardInfo, position, BorderEnum.BorderCard, 
                cardInfo.CardId.ToString() + "_origin", _positionsCardsInHand.Keys.ToList().IndexOf(position));
            _positionsCardsInHand[position] = true;
            return sprite;
        }

        public GameObject CreateHero(GameObject prefab, CardInfo heroInfo, Vector3 position)
        {
            GameObject sprite = CreateCard(prefab, heroInfo, position, 
                BorderEnum.BorderHero, heroInfo.CardId.ToString(), sortingOrderActiveCard);
            SetParent(sprite, _scaleHero);
            return sprite;
        }

        public GameObject CreateEntity(GameObject prefab, CardInfo cardInfo)
        {
            Vector3 position = _positionsActiveCards.FirstOrDefault(x => !x.Value).Key;
            GameObject sprite = CreateCard(prefab, cardInfo, position, 
                BorderEnum.BorderActiveCard, cardInfo.CardId.ToString(), sortingOrderActiveCard);
            SetParent(sprite, _scaleActiveCard);
            _positionsActiveCards[position] = true;
            return sprite;
        }

        private GameObject CreateCard(GameObject prefab, CardInfo cardInfo, Vector3 position, 
            BorderEnum border, string nameSprite, int sortingOrder)
        {
            GameObject sprite = InstantiateCard(prefab, position, cardInfo);
            LoadSprite(sprite, nameSprite, sortingOrder);            
            SetBorder(sprite, borders[border]);
            return sprite;
        }

        private GameObject InstantiateCard(GameObject prefab, Vector3 position, CardInfo cardInfo)
        {
            GameObject sprite = Instantiate(prefab, position, Quaternion.identity);
            sprite.GetComponent<ICard>().SetCard(cardInfo);
            return sprite;
        }

        private void LoadSprite(GameObject sprite, string name, int sortingOrder)
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
        private void SetBorder(GameObject sprite, GameObject border)
            => sprite.GetComponent<SelectionController>().Border = border;

        public void ClearPositionCardInHand(CardInHand card)
        {
            _positionsCardsInHand[card.transform.position] = false;
        }
    }
}

