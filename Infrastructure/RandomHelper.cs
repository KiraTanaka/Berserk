using System;

namespace Infrastructure
{
    public static class RandomHelper
    {
        private static int _seed = GetSeed();
        private static int GetSeed() => new Random().Next(int.MaxValue);

        public static Random GetRandomInstance()
        {
            if (_seed == int.MaxValue) _seed = GetSeed(); // TODO потокобезопасно?
            return new Random(_seed++);
        }

        public static int Next(int maxValue)
        {
            return GetRandomInstance().Next(maxValue);
        }
    }
}
