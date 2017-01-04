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
            var settings = ConfigurationManager.AppSettings;
            var host = settings["host"];
            var port = int.Parse(settings["port"]);
            var path = settings["path"];
            
            var context = new GameContext(RunGameCallback);
            var uri = ServerHttpConnection.BuildHttpUri(host, port, path);
            Task.Run(() => new ServerHttpConnection().Listen(uri, context.ParseRequest));
            Console.ReadLine();
        }

        private static void RunGameCallback(IEnumerable<Player> players)
        {
            const string path = @"E:\Projects\kontur\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(path, "*.dll");

            var exportedTypes = dllPaths
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes)
                .ToList();

            var cards = exportedTypes
                .SelectInstancesOf<ICardSet>()
                .SelectMany(x => x.GetSet())
                .ToList();

            cards.ForEach(x => Console.WriteLine(x.Name));

            var rules = exportedTypes
                .SelectInstancesOf<IRules>()
                .FirstOrDefault();

            Console.WriteLine($"Rows={rules?.FieldRows}, Columns={rules?.FieldColumns}, Cards={rules?.PlayerCardsAmount}");
            var playerSets = players.Select(x => new PlayerSet(x, new CardDeck(cards))).ToList();
            var game = new Game(rules, cards, playerSets);
            new Thread(() => game.Start()).Start();
        }
    }
}
