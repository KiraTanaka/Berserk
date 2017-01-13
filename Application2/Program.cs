using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application2
{
    class Program
    {
        static void Main(string[] args)
        {
            new GameConsole().Run(new StorageMock());
            Console.ReadLine();
        }
    }
}
