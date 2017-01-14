using System;

namespace Domain
{
    public static class CardExtensions
    {
        public static ICard HurtBy(this ICard card, int value)
        {
            return card.AddHealth(- value);
        }

        public static ICard Heal(this ICard card, int value)
        {
            return card.AddHealth(value);
        }

        public static ICard AddHealth(this ICard card, int value)
        {
            card.Health = card.Health + value;
            return card;
        }

        public static ICard Close(this ICard card)
        {
            card.Closed = true;
            return card;
        }

        public static ICard Open(this ICard card)
        {
            card.Closed = false;
            return card;
        }

        public static bool IsAlive(this ICard card)
        {
            return card.Health > 0;
        }

        public static Result Action(this ICard card, ActionEnum actionOption, GameState state)
        {
            if (actionOption == ActionEnum.Simple) return card.Attack(state);
            if (actionOption == ActionEnum.Feature) return card.Feature(state);
            throw new NotImplementedException($"State {state} is not implemented");
        }
    }
}
