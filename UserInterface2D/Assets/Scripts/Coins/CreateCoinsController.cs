using Domain.Process;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CreateCoinsController : MonoBehaviour {
    private List<Vector3> _positionsCoins;
    private CoordinateParser _parser = new CoordinateParser();
    string nameSprite = "Coin";
    public void LoadPositionsCoins(string namePlayer)
    {
        _positionsCoins = _parser.GetCoinPositions(namePlayer);
    }
    public void CreateCoin(GameObject prefab, string playerId)//, Coin coin)
    {
        GameObject sprite = Instantiate(prefab, _positionsCoins.First(), Quaternion.identity);
        sprite.GetComponent<CoinUnity>().PlayerId = playerId;
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(nameSprite);
        _positionsCoins.RemoveAt(0);
    }
    void LoadSprite(GameObject sprite, string name, int sortingOrder)
    {
        SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(name);
    }
}
