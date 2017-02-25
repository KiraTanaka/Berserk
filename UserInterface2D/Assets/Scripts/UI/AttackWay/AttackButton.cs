using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.UI.Controllers;
using Domain.Cards;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.AttackWay
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AttackButton : NetworkBehaviour
    {
        private ClientController _client;

        void Start()
        {
            if (!localPlayerAuthority) return;
            _client = ControllerContainer.LocalPlayerController;
        }

        void OnMouseDown()
        {
            if (!localPlayerAuthority) return;
            _client.SetAttackWay(CardActionEnum.Simple);
        }
    }
}
