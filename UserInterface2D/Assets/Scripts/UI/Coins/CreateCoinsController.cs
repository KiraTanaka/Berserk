using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI.Coordinates;
using UnityEngine;

namespace Assets.Scripts.UI.Coins
{
    public class CreateCoinsController : MonoBehaviour
    {
        private List<Vector3> _positionsCoins;

        private readonly CoordinateParser _parser = new CoordinateParser();

        private const string NameSprite = "Coin";


        public void LoadPositionsCoins(string namePlayer)
        {
            _positionsCoins = _parser.GetCoinPositions(namePlayer);
        }

        public GameObject CreateCoin(GameObject prefab, string playerId)
        {
            GameObject sprite = Instantiate(prefab, _positionsCoins.First(), Quaternion.identity);
            sprite.GetComponent<CoinUnity>().PlayerId = playerId;
            sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(NameSprite);
            _positionsCoins.RemoveAt(0);
            return sprite;
        }
    }
}
