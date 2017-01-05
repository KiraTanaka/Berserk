namespace Domain.CardData
{
    public class Currency
    {
        public bool IsGold { get; set; }
        public int Value { get; set; }
        public static Currency Gold(int value) => new Currency {IsGold = true, Value = value};
        public static Currency Silver(int value) => new Currency {IsGold = false, Value = value};

        //public static Currency operator +(Currency c1, Currency c2)
        //{
            
        //} 
    }
}