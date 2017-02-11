namespace Domain.Tools
{
    public static class RandomHelper
    {
        private static int _seed = GetSeed();
        private static int GetSeed() => new System.Random().Next(int.MaxValue);

        public static System.Random GetRandomInstance()
        {
            if (_seed == int.MaxValue) _seed = GetSeed(); // TODO потокобезопасно?
            return new System.Random(_seed++);
        }

        public static int Next(int maxValue)
        {
            return GetRandomInstance().Next(maxValue);
        }
    }
}
