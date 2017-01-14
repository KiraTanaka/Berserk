using System.Collections.Generic;

namespace Domain
{
    public class PlayerPublicInfo
    {
        public ICard Hero { get; set; }
        public List<ICard> CardsInGame { get; set; }
        public int Money { get; set; }
        public string Name { get; set; }
        public int DeckRest { get; set; }
    }
}