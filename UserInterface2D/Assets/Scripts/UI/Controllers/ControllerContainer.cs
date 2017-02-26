using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Controllers
{
    public static class ControllerContainer
    {
        public static ClientController LocalPlayerController
            => GameClientControllers.FirstOrDefault(x => x.isLocalPlayer);

        public static List<ClientController> GameClientControllers
            => GameObject
                .FindGameObjectsWithTag("Gamer")
                .Select(x => x.GetComponent<ClientController>())
                .ToList();
    }
}
