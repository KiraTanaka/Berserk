using Domain;

namespace Plugin
{
    public class Rules : IRules
    {
        public int PlayerStartActiveDeckSize { get; set; } = 4;
        public int PlayerMoneyAmount { get; set; } = 20;
    }
}
