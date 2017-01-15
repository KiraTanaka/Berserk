namespace Domain
{
    public interface IRules
    {
        int PlayerStartActiveDeckSize { get; set; }
        int PlayerStartMoneyAmount { get; set; }
        int PlayerMaxMoneyAmount { get; set; }
        int PlayerAddMoneyAmount { get; set; }
        int PlayerAddCardAmount { get; set; }
    }
}