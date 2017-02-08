using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CompleteStep : NetworkBehaviour
{
    Client client;
    private Color32 colorSelect;
    private Color32 colorOrigin;
    Image image;
    void Start()
    {
        if (!localPlayerAuthority) return;
        client = GameObject.FindGameObjectsWithTag("Gamer").Select(x => x.GetComponent<Client>())
            .FirstOrDefault(x => x.isLocalPlayer);
        image = GetComponent<Image>();
        colorOrigin = image.color;
        colorSelect = new Color32(225, 225, 225, 255);
    }
    void OnMouseDown()
    {
        if (!localPlayerAuthority) return;
        client.CompleteStep();
    }
    void OnMouseEnter()
    {
        image.color = colorSelect;
    }
    void OnMouseExit()
    {
        image.color = colorOrigin;
    }
}
