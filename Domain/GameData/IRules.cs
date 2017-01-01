namespace Domain.GameData
{
    public interface IRules
    {
        int FieldRows { get; set; }
        int FieldColumns { get; set; }
        int PlayerCardsAmount { get; set; }
    }
}