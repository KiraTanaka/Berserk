using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Net;
using Domain.BoardData;
using Domain.GameData;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = ConfigurationManager.AppSettings;
            var host = settings["host"];
            var port = int.Parse(settings["port"]);
            var path = settings["path"];

            var context = new GameContext(StartGame);
            var uri = ServerHttpConnection.BuildHttpUri(host, port, path);
            Task.Run(() => new ServerHttpConnection().Listen(uri, context.ParseRequest));
            Console.ReadLine();
        }

        private static void StartGame(IEnumerable<Player> players)
        {
            var rules = new Rules();
            var cards = new CardSet();
            var playerSets = players.Select(x => new PlayerSet(x)).ToList();
            var game = new Game(rules, cards, playerSets);
            new Thread(() => game.Start()).Start();
        }
    }
}
