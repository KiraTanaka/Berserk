using Domain.Process;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateCoinsController : MonoBehaviour {
    private List<Vector3> _positionsCoins;
    private CoordinateParser _parser = new CoordinateParser();
    string nameSprite = "Coin";
    public void LoadPositionsCoins(string name)
    {
        _positionsCoins = _parser.Parse(name);
    }
    public void CreateCoin(GameObject prefab, Coin coin)
    {
        GameObject sprite = Instantiate(prefab, _positionsCoins.First(), Quaternion.identity);
        sprite.GetComponent<CoinUnity>().SetCoin(coin);
        sprite.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(nameSprite);
        _positionsCoins.RemoveAt(0);
    }
    void LoadSprite(GameObject sprite, string name, int sortingOrder)
    {
        SpriteRenderer renderer = sprite.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(name);
    }
}
