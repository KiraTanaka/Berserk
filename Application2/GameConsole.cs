using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;
using Domain.Process;
using Infrastructure.Loop;

namespace Application2
{
    public class GameConsole : Game
    {
        public GameConsole(IStorage storage, IRules rules, List<Card> cards)
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
            players.ForEach(x => Console.WriteLine($"Player {x.Name} with {x.FullDeck.Length} cards added"));
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
            Console.WriteLine("------------------------- GAME STATE -------------------------");
            Console.WriteLine("{0}\t\t\t\t\t\t{1}", "CURRENT", "ANOTHER");
            Console.WriteLine("{0}\t\t\t\t\t\t{1}", current.Name, another.Name);
            Console.WriteLine("{0}\t{1}\t\t\t\t\t{2}\t{3}", current.Hero.Name, current.Hero.Health, another.Hero.Name, another.Hero.Health);
            
            var currentCardsInGame = CardsToLine(current.CardsInGame);
            var anotherCardsInGame = CardsToLine(another.CardsInGame);
            Console.WriteLine("{0}", "Cards in game (Power/Health)");
            Console.WriteLine("{0}\t\t\t\t\t\t{1}", currentCardsInGame.Item1, anotherCardsInGame.Item1);
            Console.WriteLine("{0}\t\t\t\t\t\t{1}", currentCardsInGame.Item2, anotherCardsInGame.Item2);

            var currentActiveDeck = CardsToLine(current.ActiveDeck);
            Console.WriteLine("{0}", "Cards on hands (Power/Health)");
            Console.WriteLine("{0}", currentActiveDeck.Item1);
            Console.WriteLine("{0}", currentActiveDeck.Item2);

            Console.WriteLine();
        }

        private static Tuple<string, string> CardsToLine(IEnumerable<Card> cards)
        {
            var v = 1;
            var header = new StringBuilder();
            var line = new StringBuilder();
            cards.ForEach(x =>
            {
                header.Append($"{v++}\t");
                line.Append($"{x.Power}/{x.Health}\t");
            });
            return new Tuple<string, string>(header.ToString(), line.ToString());
        }

        public override void ShowWinner(Player winner)
        {
            Console.WriteLine($"Player {winner.Name} win");
        }

        public override Card GetActionCard(Player actionPlayer)
        {
            Console.Write("Enter your card > ");
            var index = ReadNumber();
            return SelectCard(index, actionPlayer, actionPlayer.ActiveDeck);
        }

        public override List<Card> GetTargetCards(Player targetPlayer)
        {
            Console.Write("Enter target cards > ");
            var indexes = ReadNumbers();
            return SelectCards(indexes, targetPlayer, targetPlayer.CardsInGame);
        }

        public override CardActionEnum GetAttackWay()
        {
            Console.Write("Enter attack way > ");
            var attackWay = ReadNumber();
            return (CardActionEnum) attackWay;
        }

        public override void ShowActionResult(MoveResult moveResult)
        {
            Console.WriteLine($"Success: {moveResult.Success}, Message: {moveResult.Message}");
        }

        public override void InformAboutAttack(
            Card actionCard, IEnumerable<Card> targetCards, CardActionEnum actionWay)
        {
            Console.WriteLine($"You card {actionCard.Name} is going to attack {targetCards.ToStringNames()}");
        }

        private static List<Card> SelectCards(int[] indexes, Player player, Card[] cards)
        {
            var result = new List<Card>();
            indexes.ForEach(x =>
            {
                var card = SelectCard(x, player, cards);
                if (card != null) result.Add(card);
            });
            return result;
        }

        private static Card SelectCard(int index, Player player, Card[] cards)
        {
            if (index >= 0 || index < cards.Length)
                return index == 0 ? player.Hero : cards.ElementAt(index-1);
            return null;
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
        public static string ToStringAsList(this IEnumerable<Card> deck, bool markers)
        {
            var deckStr = new StringBuilder();
            var counter = 1;
            deck.ForEach(x => deckStr.Append(markers ? $"{counter++} {x.Name}\n" : $"{x.Name}\n"));
            return deckStr.ToString();
        }

        public static string ToStringNames(this IEnumerable<Card> deck)
        {
            var deckStr = new StringBuilder();
            deck.ForEach(x => deckStr.Append($"{x.Name}, "));
            return deckStr.ToString();
        }
    }
}
