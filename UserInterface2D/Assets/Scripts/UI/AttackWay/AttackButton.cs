using UnityEngine;
using Domain.Cards;
using System.Linq;
using UnityEngine.Networking;

public class AttackButton : NetworkBehaviour
{
    Client client;
    void Start () {
        if (!localPlayerAuthority) return;
        client = GameObject.FindGameObjectsWithTag("Gamer").Select(x => x.GetComponent<Client>())
            .FirstOrDefault(x => x.isLocalPlayer);
    }
    void OnMouseDown()
    {
        if (!localPlayerAuthority) return;
        client.SetAttackWay(CardActionEnum.Simple);
    }
}
