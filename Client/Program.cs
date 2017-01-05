using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Net;
using Domain.CardData;
using Newtonsoft.Json;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = ConfigurationManager.AppSettings;
            var host = settings["host"];
            var port = int.Parse(settings["port"]);
            var path = settings["path"];

            var uri = ClientHttpConnection.BuildHttpUri(host, port, path);
            var connection = new ClientHttpConnection(uri);
            Game(connection);
            Console.ReadLine();
        }

        private static void Game(ClientHttpConnection connection)
        {
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();

            Registration(connection, userId1);

            Thread.Sleep(500);
            Registration(connection, userId2);

            Thread.Sleep(500);
            Move(connection, userId1);

            Thread.Sleep(500);
            Move(connection, userId2);
        }

        private static void Move(ClientHttpConnection connection, Guid userId)
        {
            var request = new Request
            {
                Name = "User" + userId.ToString().Last(),
                UserId = userId,
                Registration = false,
                Move = null
            };
            Task.Run(() => connection.PostAsync(request, x => Callback(x, userId)));
        }

        private static void Registration(ClientHttpConnection connection, Guid userId)
        {
            var request = new Request
            {
                Name = "User" + userId.ToString().Last(),
                UserId = userId,
                Registration = true,
                Move = null
            };
            Task.Run(() => connection.PostAsync(request, x => Callback(x, userId)));
        }

        private static void Callback(string response, Guid userId)
        {
            try
            {
                var serverResponse = JsonConvert.DeserializeObject<Responce>(response);
                var playerZone = serverResponse?.GameInfo?.PlayerZoneInfos?.FirstOrDefault(x => x.PlayerId == userId);
                var cards = playerZone?.Desk;
                if (cards != null)
                {
                    Console.WriteLine("Your cards:");
                    foreach (var card in cards)
                    {
                        Console.WriteLine(card.Name);
                    }
                }
                Console.WriteLine(serverResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
