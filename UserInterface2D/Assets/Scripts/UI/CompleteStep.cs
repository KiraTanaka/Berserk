using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteStep : MonoBehaviour
{
    GameScript game;
    // Use this for initialization
    void Start()
    {
        game = GameObject.FindWithTag("Scripts").GetComponent<GameScript>();
    }
    void OnMouseDown()
    {
        game.CompleteStep();
    }
}
