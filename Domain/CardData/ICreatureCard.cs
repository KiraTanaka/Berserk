namespace Domain.CardData
{
    public interface ICreatureCard : IBaseCard
    {
        int Health { get; set; }
        int Range { get; set; }
        Attack Attack { get; set; }
    }
}
