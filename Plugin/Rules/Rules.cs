using Domain.GameData;

namespace Plugin.Rules
{
    public class Rules : IRules
    {
        public int FieldRows => 6;
        public int FieldColumns => 5;
        public int PlayerCardsAmount => 3;
        public int CurrencyAmount => 25;
    }
}
