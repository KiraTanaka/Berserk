using Assets.Scripts.Application.Cards;

namespace Assets.Scripts.Application.Players
{
    public interface IPlayer
    {
        string GetId();

        void OnStartPlayer(string playerId, CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin);

        void LocateCards(CardInfo heroInfo, CardInfo[] cardsInfo, int countCoin);

        void OnAddCoin();

        void CreateActiveCard(CardInfo cardInfo, string playerId);

        void DestroyCardInHand(string instId, string playerId);

        void CloseCoins(int count, string playerId);

        void OnChangeHealth(string instId, int health);

        void OnChangeClosed(string instId, bool closed);

        void OpenAll(string playerId);

        void UpdateCountCoins(string[] playersId, int[] countCoinsPlayers);
    }
}