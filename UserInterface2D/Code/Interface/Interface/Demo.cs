using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Plugin;
using UnityEngine;

namespace Interface
{
    public class Demo : MonoBehaviour
    {
        Card87242 card;
        void Start()
        {
            card = new Card87242();
        }
        void Update()
        {
            if (card?.Cost > 0)
                Debug.Log("egweg");
        }
    }
}
