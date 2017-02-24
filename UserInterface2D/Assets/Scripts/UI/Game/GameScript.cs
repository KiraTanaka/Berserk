using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Infrastructure;
using Assets.Scripts.UI.Controllers;
using Domain.Cards;
using Domain.GameProcess;
using UnityEngine.Networking;

namespace Assets.Scripts.UI.Game
{
    public class GameScript : NetworkBehaviour
    {
        private int _playerCount;

        private GameUnity _game;

        
        // Use this for initialization
        // ReSharper disable once UnusedMember.Local
        void Awake()
        {
            var types = ImportTypes().ToList();
            var rules = types.SelectInstancesOf<IRules>()?.FirstOrDefault();
            var cards = types.SelectInstancesOf<Card>().ToList();
            var users = GetUsers();

            _game = new GameUnity(rules, cards, users);
            _game.Run();
        }

        private static IEnumerable<Type> ImportTypes()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Resources");
            var dllPaths = Directory.GetFiles(path, "*.dll");
            return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.GetExportedTypes());
        }

        private static UserLimitedSet GetUsers()
        {
            var storage = new DummyStorage();
            var user1 = storage.FindById<User>(1).First();
            var user2 = storage.FindById<User>(2).First();
            return new UserLimitedSet { user1, user2 };
        }

        public Player GetPlayer()
        {
            if (_playerCount >= 2) _playerCount = 0;

            return _playerCount++ == 0 ? _game.State.MovingPlayer : _game.State.WaitingPlayer;
        }
        public void ConnectPlayer(NetworkInstanceId networkId)
        {
            var state = _game.State;
            Player player = GetPlayer();           
            var serverController = GetComponent<ServerController>();
            serverController.PlayerInitialization(player, networkId);            
            serverController.EnemyInitialization(
                (state.MovingPlayer.Id == player.Id) ? state.WaitingPlayer : state.MovingPlayer, networkId);
        }
        public GameUnity GetGame()
        {
            return _game;
        }
    }
}