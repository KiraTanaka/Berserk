using Domain;
using Domain.Process;

namespace Plugin
{
    public class Rules : IRules
    {
        public int PlayerStartActiveDeckSize { get; set; } = 4;
        public int PlayerStartMoneyAmount { get; set; } = 1;
        public int PlayerMaxMoneyAmount { get; set; } = 10;
        public int PlayerAddMoneyAmount { get; set; } = 1;
        public int PlayerAddCardAmount { get; set; } = 1;
    }
}
