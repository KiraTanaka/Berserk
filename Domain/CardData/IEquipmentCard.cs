namespace Domain.CardData
{
    public interface IEquipmentCard : IBaseCard
    {
        EquipmentEnum Type { get; }
    }
}
