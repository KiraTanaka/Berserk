using System.Linq;
using Assets.Scripts.Infrastructure;
using UnityEngine;

namespace Assets.Scripts.UI.AttackWay
{
    public static class GameObjectHelper
    {
        public static ClientController FindGamerClient()
        {
            return GameObject
                .FindGameObjectsWithTag("Gamer")
                .Select(x => x.GetComponent<ClientController>())
                .FirstOrDefault(x => x.isLocalPlayer);
        }
    }
}
