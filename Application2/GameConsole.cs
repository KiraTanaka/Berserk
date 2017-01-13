using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Domain;
using Microsoft.Build.Tasks;

namespace Application2
{
    public class GameConsole : Game
    {
        public override IEnumerable<Type> ImportTypes()
        {
            const string dllFolderPath = @"E:\Projects\kontur\Berserk\Plugins";
            var dllPaths = Directory.GetFiles(dllFolderPath, "*.dll");
            return dllPaths.Select(Assembly.LoadFrom).SelectMany(x => x.ExportedTypes);
        }

        public override IEnumerable<User> LoadUsers()
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
                    var player = Storage.FindById<User>(id)?.FirstOrDefault();
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

        public override IEnumerable<Player> LoadPlayers(IEnumerable<User> users)
        {
            var players = new List<Player>();

            foreach (var user in users)
            {
                var playerCards = user.CardList.Select(id => Cards.FirstOrDefault(x => x.Id == id)).ToList();

                var deck = new CardDeck(playerCards);
                var player = new Player(user, deck, Rules);

                players.Add(player);
                Console.WriteLine($"Player {player.User.Name} with {player.FullDeck.Rest} cards added");
            }

            return players;
        }

        public override Player SelectFirst(IEnumerable<Player> players)
        {
            var playersArr = players.ToArray();
            var firstPlayerIndex = new Random().Next(playersArr.Length);
            return playersArr[firstPlayerIndex];
        }

        public override void OfferToChangeCards(IEnumerable<Player> players)
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
                    catch (Exception)
                    {
                        Console.WriteLine("Incorrect input");
                    }
                }
            }
        }

        private bool _isFirstMove = true;

        public override void Play(IEnumerable<Player> players)
        {
            if (_isFirstMove)
            {
                _isFirstMove = false;
                AddPlayersCardAndMoney(players);
                Move();
            }
            Move();
        }

        private void AddPlayersCardAndMoney(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                player.AddMoney();
                player.DealCard();
            }
        }

        private void Move()
        {
            
        }
    }
}
