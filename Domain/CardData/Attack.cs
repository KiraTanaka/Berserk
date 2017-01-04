namespace Domain.CardData
{
    public class Attack
    {
        public int Low { get; set; }
        public int Mid { get; set; }
        public int High { get; set; }

        public Attack(int low, int mid, int high)
        {
            Low = low;
            Mid = mid;
            High = high;
        }
    }
}
