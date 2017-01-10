namespace Domain
{
    public static class CardExtension
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
    }
}
