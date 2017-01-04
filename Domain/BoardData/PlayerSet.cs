using Domain.CardData;
using Domain.GameData;

namespace Domain.BoardData
{
    /// <summary>
    /// Представляет собой набор данных связанных с игроком.
    /// Игрок, колода, вспомогательная территория, кладбище.
    /// </summary>
    public class PlayerSet
    {
        private readonly Player _player;
        private readonly PlayerCardSet _desk;
        private readonly PlayerCardSet _utilityArea;
        private readonly PlayerCardSet _cemetery;

        public PlayerSet(Player player)
        {
            _player = player;
            _desk = new PlayerCardSet(player.Id);
            _utilityArea = new PlayerCardSet(player.Id);
            _cemetery = new PlayerCardSet(player.Id);
        }

        public void DealCard(IBaseCard card)
        {
            _desk.Push(card);
        }

        public IBaseCard SelectCard(GameInfo gameInfo, CardSet cardSet)
        {
            return _player.SelectCard(gameInfo, cardSet);
        }

        public PlayerMove Move(GameInfo gameInfo)
        {
            return _player.Move(gameInfo);
        }

        public PlayerSetInfo GetInfo()
        {
            return PlayerSetInfo
                .SetDeskInfo(_desk.GetInfo())
                .SetUtilityAreaInfo(_utilityArea.GetInfo())
                .SetCementeryInfo(_cemetery.GetInfo())
                .SetPlayerId(_player.Id)
                .GetPlayerSetInfo();
        }
    }
}
