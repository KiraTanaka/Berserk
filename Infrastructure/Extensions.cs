﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public static class Extensions
    {
        /// <summary>
        /// Fisher–Yates shuffle.
        /// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        /// http://stackoverflow.com/questions/273313/randomize-a-listt
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var random = RandomHelper.GetRandomInstance();
            var array = enumerable.ToArray();
            for (var i = 0; i < array.Length; i++)
                Swap(ref array[i], ref array[random.Next(i, array.Length)]);
            foreach (var item in array)
                yield return item;
        }

        public static void Swap<T>(ref T val1, ref T val2)
        {
            var temp = val1;
            val1 = val2;
            val2 = temp;
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable) action(item);
        }

        public static void ForLoop(this int limit, Action<int> action)
        {
            for (var i = 0; i < limit; i++) action(i);
        }

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
