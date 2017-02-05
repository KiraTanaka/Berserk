using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Cards;
using Domain.Process;

public class GameUnity : Game
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
        return _num++;// ReadNumber();
    }

    public override void ShowPlayers(IEnumerable<Player> players) { }

    public override void OfferToChangeCards(IEnumerable<Player> players) { }

    public override void ShowInfo(Player current, Player another) { }

    public override void ShowWinner(Player winner)
    {
       // Console.WriteLine($"Player {winner.Name} win");
    }

    public override Card GetActionCard(Player actionPlayer)//не используется
    {
        return null;
    }

    public override List<Card> GetTargetCards(Player targetPlayer)//не используется
    {
        return null;
    }

    public override CardActionEnum GetAttackWay()
    {
        return gamescript.AttackWay;
    }

    public override void ShowActionResult(MoveResult moveResult)
    {
        //Console.WriteLine($"Success: {moveResult.Success}, Message: {moveResult.Message}");
    }

    public override void InformAboutAttack(
        Card actionCard, IEnumerable<Card> targetCards, CardActionEnum actionWay)
    {
       // Console.WriteLine($"You card {actionCard.Name} is going to attack {targetCards.ToStringNames()}");
    }  
}

public static class Extensions
{
    public static string ToStringAsList(this IEnumerable<Card> deck, bool markers)
    {
        var deckStr = new StringBuilder();
        var counter = 1;
        foreach (var x in deck)
        {
            deckStr.Append(markers ? $"{counter++} {x.Name}\n" : $"{x.Name}\n");
        }
        return deckStr.ToString();
    }

    public static string ToStringNames(this IEnumerable<Card> deck)
    {
        var deckStr = new StringBuilder();
        foreach (var x in deck)
        {
            deckStr.Append($"{x.Name}, ");
        }
        return deckStr.ToString();
    }
}

