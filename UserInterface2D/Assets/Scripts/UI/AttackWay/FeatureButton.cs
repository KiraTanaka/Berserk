using UnityEngine;
using Domain.Cards;

public class FeatureButton : MonoBehaviour {
    GameScript game;
    void Start()
    {
        game = GameObject.FindWithTag("Scripts").GetComponent<GameScript>();
    }
    void OnMouseDown()
    {
        game.AttackWay = CardActionEnum.Feature;
    }
}
