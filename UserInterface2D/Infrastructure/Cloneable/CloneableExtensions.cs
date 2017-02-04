using System;

namespace Infrastructure.Cloneable
{
    public static class CloneableExtensions
    {
        public static T Clone<T>(this ICloneable<T> cloneable, Action<T> action)
        {
            return cloneable.Clone(player =>
            {
                action(player);
                return player;
            });
        }

        public static T Clone<T>(this ICloneable<T> cloneable, Func<T, T> func)
        {
            var clone = cloneable.Clone();
            return func(clone);
        }
    }
}
