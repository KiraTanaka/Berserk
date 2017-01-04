using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Net;
using Domain.BoardData;
using Domain.CardData;
using Domain.GameData;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string path = @"E:\Projects\kontur\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(path, "*.dll");

            foreach (var dllPath in dllPaths)
                ExecutePluginFromDll(dllPath);

            //            var settings = ConfigurationManager.AppSettings;
            //            var host = settings["host"];
            //            var port = int.Parse(settings["port"]);
            //            var path = settings["path"];
            //
            //            var context = new GameContext(StartGame);
            //            var uri = ServerHttpConnection.BuildHttpUri(host, port, path);
            //            Task.Run(() => new ServerHttpConnection().Listen(uri, context.ParseRequest));
            Console.ReadLine();
        }

        private static void ExecutePluginFromDll(string dllPath)
        {
            var assembly = Assembly.LoadFrom(dllPath);

            var plugins = assembly.ExportedTypes
                .Where(IsCardSet)
                .Select(GetInstance)
                .ToList();

            var cards = plugins.SelectMany(x => x.GetSet()).ToList();
            cards.ForEach(x => Console.WriteLine(x.Name));
            //plugins.ForEach(x => Console.WriteLine(x.Name));
        }

        private static bool IsCardSet(Type t)
        {
            return typeof(ICardSet).IsAssignableFrom(t)
                && t.GetConstructor(Type.EmptyTypes) != null;
        }

        private static ICardSet GetInstance(Type t)
        {
            return (ICardSet)Activator.CreateInstance(t);
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
