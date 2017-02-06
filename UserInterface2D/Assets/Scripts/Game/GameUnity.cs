using System.Collections.Generic;
using System.Text;
using Domain.Cards;
using Domain.Process;

public class GameUnity : Game
{
    private readonly GameScript _gamescript;

    public GameUnity(GameScript gamescript, IRules rules, IEnumerable<Card> cards, UserLimitedSet users)
        : base(rules, cards, users)
    {
        _gamescript = gamescript;
    }
    
    public override CardActionEnum GetAttackWay()
    {
        return _gamescript.AttackWay;
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

