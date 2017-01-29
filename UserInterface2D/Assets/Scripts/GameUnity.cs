using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;
using Domain.Process;
using Infrastructure.Loop;

public class GameUnity:Game
{
    GameScript gamescript;
    public GameUnity(GameScript gamescript, IStorage storage, IRules rules, List<Card> cards)
            : base(storage, rules, cards)
        {
        this.gamescript = gamescript;
    }

    private int _num = 1;
    public override int ConnectUser()
    {
        Console.Write("Enter player ID > ");
        return _num++;// ReadNumber();
    }

    public override void ShowPlayers(IEnumerable<Player> players) { }

    public override void OfferToChangeCards(IEnumerable<Player> players) { }

    public override void ShowInfo(Player current, Player another) { }

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
        /*Console.Write("Enter attack way > ");
        var attackWay = ReadNumber();*/
        return gamescript.AttackWay;//)attackWay;
    }

    public override void ShowActionResult(MoveResult moveResult)
    {
        Console.WriteLine($"Success: {moveResult.Success}, Message: {moveResult.Message}");
    }

    public override void InformAboutAttack(
        Card actionCard, IEnumerable<Card> targetCards, CardActionEnum actionWay)
    {
       // Console.WriteLine($"You card {actionCard.Name} is going to attack {targetCards.ToStringNames()}");
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
            return index == 0 ? player.Hero : cards.ElementAt(index - 1);
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
                var splitted = read.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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

