using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Net;
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
            
            var context = new HttpContext();
            var uri = ServerHttpConnection.BuildHttpUri(host, port, path);
            Task.Run(() => new ServerHttpConnection().Listen(uri, context.ParseRequest));

            const string dllFolderPath = @"E:\Projects\kontur\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(dllFolderPath, "*.dll");

            var exportedTypes = dllPaths
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes)
                .ToList();

            var cards = exportedTypes
                .SelectInstancesOf<ICardSet>()
                .SelectMany(x => x)
                .ToList();

            cards.ForEach(x => Console.WriteLine(x.Name));

            var rules = exportedTypes
                .SelectInstancesOf<IRules>()
                .FirstOrDefault();

            Console.WriteLine($"Rows={rules?.FieldRows}, Columns={rules?.FieldColumns}, Cards={rules?.PlayerCardsAmount}");

            new Game(rules, cards, context);

            Console.ReadLine();
        }
    }
}
