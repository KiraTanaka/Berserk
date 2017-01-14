using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain;

namespace Application2
{
    public class GameConsole : Game
    {
        public GameConsole(IStorage storage, IRules rules, List<ICard> cards)
            : base(storage, rules, cards)
        {
        }

        private int _num = 1;
        public override int ConnectUser()
        {
            Console.Write("Enter player ID > ");
            return _num++;// ReadNumber();
        }

        public override void ShowPlayers(IEnumerable<Player> players)
        {
            Console.WriteLine();
            players.ForEach(x => Console.WriteLine($"Player {x.Name} with {x.FullDeck.Rest} cards added"));
        }
        
        public override void OfferToChangeCards(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                Console.WriteLine();
                Console.WriteLine($"Player {player.Name}");
                Console.WriteLine("Full deck cards:");
                Console.WriteLine(player.FullDeck.ToStringAsList(true));
                Console.WriteLine("Active deck cards:");
                Console.WriteLine(player.ActiveDeck.ToStringAsList(true));
                Console.Write("Enter numbers of cards you want to change > ");

                var toMove = ReadNumbers();
                if (toMove.Length == 0) continue;
                player.RedealCards(toMove);

                Console.WriteLine("Your cards now:");
                Console.WriteLine("Full deck cards:");
                Console.WriteLine(player.FullDeck.ToStringAsList(true));
                Console.WriteLine("Active deck cards:");
                Console.WriteLine(player.ActiveDeck.ToStringAsList(true));
            }
        }

        public override void ShowInfo(Player current, Player another)
        {
            Console.WriteLine();
            Console.WriteLine($"{current.Name} info ---------------------------------------");
            Console.WriteLine("Your info:");
            Console.WriteLine(current.ToStringInfo());
            Console.WriteLine("Your active deck:");
            Console.WriteLine(current.ActiveDeck.ToStringAsList(true));

            Console.WriteLine($"{another.Name} info:");
            Console.WriteLine(another.ToStringInfo());
        }

        public override void ShowWinner(Player winner)
        {
            Console.WriteLine($"Player {winner.Name} win");
        }

        public override ICard GetActionCard(Player actionPlayer)
        {
            Console.Write("Enter your card > ");
            var index = ReadNumber();
            return SelectCard(index, actionPlayer, actionPlayer.ActiveDeck);
        }

        public override IEnumerable<ICard> GetTargetCards(Player targetPlayer)
        {
            Console.Write("Enter target cards > ");
            var indexes = ReadNumbers();
            return SelectCards(indexes, targetPlayer, targetPlayer.CardsInGame);
        }

        public override ActionEnum GetAttackWay()
        {
            Console.Write("Enter attack way > ");
            var attackWay = ReadNumber();
            return (ActionEnum) attackWay;
        }

        public override void ShowActionResult(Result result, Player another)
        {
            Console.WriteLine($"Success: {result.Success}, Message: {result.Message}");
            Console.WriteLine(another.Hero.Health);
        }

        public override void InformAboutAttack(
            ICard actionCard, IEnumerable<ICard> targetCards, ActionEnum actionWay)
        {
            Console.WriteLine($"You card {actionCard.Name} is going to attack {targetCards.ToStringNames()}");
        }

        private static List<ICard> SelectCards(int[] indexes, Player player, IEnumerable<ICard> cards)
        {
            var result = new List<ICard>();
            indexes.ForEach(x => result.Add(SelectCard(x, player, cards)));
            return result;
        }

        private static ICard SelectCard(int index, Player player, IEnumerable<ICard> cards)
        {
            return index == 0 ? player.Hero : cards.ElementAt(index-1);
        }

        private static int ReadNumber()
        {
            while (true)
            {
                var numbers = ReadNumbers();
                if (numbers.Length == 0) Console.Write("Incorrect imput, try again > ");
                else return numbers[0];
            }
        }

        private static int[] ReadNumbers()
        {
            return ReadNumbers(Console.ReadLine, Console.Write);
        }

        private static int[] ReadNumbers(Func<string> source, Action<string> errorTarget)
        {
            while (true)
            {
                try
                {
                    var read = (source() ?? "").Trim();
                    var splitted = read.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                    return splitted.Select(int.Parse).ToArray();
                }
                catch (Exception)
                {
                    errorTarget("Incorrect imput, try again > ");
                }
            }
        }
    }

    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action(item);
        }

        public static string ToStringAsList(this IEnumerable<ICard> deck, bool markers)
        {
            var deckStr = new StringBuilder();
            var counter = 1;
            deck.ForEach(x => deckStr.Append(markers ? $"{counter++} {x.Name}\n" : $"{x.Name}\n"));
            return deckStr.ToString();
        }

        public static string ToStringNames(this IEnumerable<ICard> deck)
        {
            var deckStr = new StringBuilder();
            deck.ForEach(x => deckStr.Append($"{x.Name}, "));
            return deckStr.ToString();
        }

        public static string ToStringInfo(this Player player)
        {
            var sb = new StringBuilder();
            sb.Append($"User: {player.Name}\n");
            sb.Append($"Hero: {player.Hero.Name}\n");
            sb.Append($"Health: {player.Hero.Health}\n");
            sb.Append($"Money: {player.Money}\n");
            sb.Append($"Cards on board:\n{player.CardsInGame.ToStringAsList(true)}");
            return sb.ToString();
        }
    }
}
