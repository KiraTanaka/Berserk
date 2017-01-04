namespace Domain.CardData
{
    public interface ICreatureCard : IBaseCard
    {
        int Health { get; }
        int Range { get; }
        Attack Attack { get; }
        string Moto { get; }
    }
}
