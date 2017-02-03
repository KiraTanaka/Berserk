using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CompleteStep : NetworkBehaviour
{
    Client client;
    void Start()
    {
        if (!localPlayerAuthority) return;
        client = GameObject.FindGameObjectsWithTag("Gamer").Select(x => x.GetComponent<Client>())
            .FirstOrDefault(x => x.isLocalPlayer);
    }
    void OnMouseDown()
    {
        if (!localPlayerAuthority) return;
        client.CompleteStep();
    }
}
