using System;
using System.Collections.Generic;
using System.Configuration;
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
            var appSettings = ConfigurationManager.AppSettings;
            var host = appSettings["host"];
            var port = int.Parse(appSettings["port"]);
            var path = appSettings["path"];

            var gameContext = new GameContext(StartGame);
            var uri = ServerHttpConnection.BuildHttpUri(host, port, path);
            Task.Run(() => new ServerHttpConnection().Listen(uri, gameContext.ParseRequest));
            Console.ReadLine();
        }

        private static void StartGame(List<PlayerSet> playerSets)
        {
            new Game(new Rules(), new CardSet(), playerSets).Start();
        }
    }
}
