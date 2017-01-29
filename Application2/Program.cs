using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Domain.Cards;
using Domain.Process;

namespace Application2
{
    public class Program
    {
        static void Main(string[] args)
        {
            var storage = new StorageMock();
            var types = ImportTypes().ToList();
            var rules = types.SelectInstancesOf<IRules>()?.FirstOrDefault();
            var cards = types.SelectInstancesOf<Card>().ToList();

            new GameConsole(storage, rules, cards).Run();
            Console.ReadLine();
        }

        private static IEnumerable<Type> ImportTypes()
        {
            const string dllFolderPath = @"C:\Users\Akira\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(dllFolderPath, "*.dll");
            return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.ExportedTypes);
        }
    }
}
