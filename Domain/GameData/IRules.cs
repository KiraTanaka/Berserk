namespace Domain.GameData
{
    public interface IRules
    {
        int FieldRows { get; }
        int FieldColumns { get; }
        int PlayerCardsAmount { get; }
        int GoldAmount { get; }
        int SilverAmount { get; }
    }
}