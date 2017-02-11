using System.Linq;
using Assets.Scripts.Infrastructure;
using UnityEngine;

namespace Assets.Scripts.UI.AttackWay
{
    public static class GameObjectHelper
    {
        public static Client FindGamerClient()
        {
            return GameObject
                .FindGameObjectsWithTag("Gamer")
                .Select(x => x.GetComponent<Client>())
                .FirstOrDefault(x => x.isLocalPlayer);
        }
    }
}
