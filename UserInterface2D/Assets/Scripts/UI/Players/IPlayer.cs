using Assets.Scripts.UI.Cards;

namespace Assets.Scripts.UI.Players
{
    public interface IPlayer
    {
        string GetId();

        void OnStartPlayer(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin);

        void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin);

        void CreateActiveCard(CardInfo cardInfo, string playerId);

        void DestroyCardInHand(string instId, string playerId);

        void CloseCoins(int count, string playerId);

        void OnChangeHealthCard(string instId, int health);

        void OnChangeClosedCard(string instId, bool closed);

        void OpenAll(string playerId);

        void UpdateCountCoins(string playerId, int countCoinsPlayer);
    }
}