using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Domain;
using Microsoft.Build.Tasks;

namespace Application2
{
    public class Game
    {
        public Game()
        {
            const string dllFolderPath = @"E:\Projects\kontur\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(dllFolderPath, "*.dll");

            var exportedTypes = dllPaths
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.ExportedTypes)
                .ToList();

            var cards = exportedTypes.SelectInstancesOf<ICard>().ToList();
            var rules = exportedTypes.SelectInstancesOf<IRules>()?.FirstOrDefault();
            var storage = new StorageMock();
            cards.ForEach(x => Console.WriteLine(x.Name));

            var users = ConnectUsers(storage);
            var players = CreatePlayers(users, cards, rules).ToList();
            var player = SelectFirst(players);
            AskPlayerAboutCards(players);
            Move(players);

            Console.WriteLine();
            Console.WriteLine($"First user is {player.User.Id}");
        }

        private static void Move(IEnumerable<Player> players)
        {
            
        }

        private static void AskPlayerAboutCards(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                var activeDeck = new StringBuilder();
                var counter = 0;
                foreach (var card in player.ActiveDeck)
                    activeDeck.Append($"{counter++} {card.Name}, ");

                var fullDeck = new StringBuilder();
                foreach (var card in player.FullDeck)
                    fullDeck.Append($"{card.Name}, ");

                Console.WriteLine();
                Console.WriteLine($"Player {player.User.Name}");
                Console.WriteLine("Full deck cards:");
                Console.WriteLine(fullDeck);
                Console.WriteLine("Active deck cards:");
                Console.WriteLine(activeDeck);
                Console.Write("Enter numbers of cards you want to change > ");

                var answer = Console.ReadLine();
                if (string.IsNullOrEmpty(answer))
                    continue;

                while (true)
                {
                    var splitted = answer.Split(' ').Select(x => int.Parse(x.Trim())).ToArray();
                    try
                    {
                        player.RedealCards(splitted);

                        activeDeck = new StringBuilder();
                        counter = 0;
                        foreach (var card in player.ActiveDeck)
                            activeDeck.Append($"{counter++} {card.Name}, ");

                        fullDeck = new StringBuilder();
                        foreach (var card in player.FullDeck)
                            fullDeck.Append($"{card.Name}, ");

                        Console.WriteLine("Ok your cards now:");
                        Console.WriteLine("Full deck cards:");
                        Console.WriteLine(fullDeck);
                        Console.WriteLine("Active deck cards:");
                        Console.WriteLine(activeDeck);

                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Incorrect input");
                    }
                }
            }
        }

        private static Player SelectFirst(IEnumerable<Player> players)
        {
            var playersArr = players.ToArray();
            var firstPlayerIndex = new Random().Next(playersArr.Length);
            return playersArr[firstPlayerIndex];
        }

        private static IEnumerable<Player> CreatePlayers(IEnumerable<User> users, IEnumerable<ICard> allowedCards, IRules rules)
        {
            var players = new List<Player>();

            foreach (var user in users)
            {
                var playerCards = new List<ICard>();
                foreach (var cardId in user.Cards)
                {
                    var card = allowedCards.FirstOrDefault(x => x.Id == cardId);
                    playerCards.Add(card);
                }

                var deck = new CardDeck(playerCards);
                var player = new Player(user, deck, rules);

                players.Add(player);
                Console.WriteLine($"Player {player.User.Name} with {player.FullDeck.Rest} cards added");
            }

            return players;
        }

        private static IEnumerable<User> ConnectUsers(IStorage storage)
        {
            var users = new List<User>();
            Console.Write("Enter player ID > ");
            while (true)
            {
                var read = Console.ReadLine();
                int id;
                var parsed = int.TryParse(read, out id);
                if (parsed)
                {
                    var player = storage.FindById<User>(id)?.FirstOrDefault();
                    if (player == null)
                    {
                        Console.Write("Unknown player. Enter player ID > ");
                    }
                    else
                    {
                        users.Add(player);
                        Console.WriteLine($"User {player.Name} connected");
                        Console.WriteLine();
                        if (users.Count < 2)
                        {
                            Console.Write("Enter another player ID > ");
                        }
                        else
                        {
                            Console.WriteLine("Users connected");
                            Console.WriteLine();
                            return users;
                        }
                    }
                }
                else
                {
                    Console.Write("Incorrect ID. Enter player ID > ");
                }
            }
        }
    }
}
