using UnityEngine;
using Domain.Cards;

public class AttackButton : MonoBehaviour {
    GameScript game;
    // Use this for initialization
    void Start () {
        game = GameObject.FindWithTag("Scripts").GetComponent<GameScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseDown()
    {
        game.AttackWay = CardActionEnum.Simple;
    }
}
