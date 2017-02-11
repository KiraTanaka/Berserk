using System;
using System.Collections.Generic;

namespace Domain.Tools
{
    public static class LoopExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action(item);
        }

        public static void ForLoop(this int limit, Action<int> action)
        {
            for (var i = 0; i < limit; i++) action(i);
        }
    }
}
