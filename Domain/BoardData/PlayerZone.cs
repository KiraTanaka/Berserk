using System;
using Domain.CardData;
using Domain.GameData;

namespace Domain.BoardData
{
    /// <summary>
    /// Представляет собой набор данных связанных с игроком.
    /// Игрок, колода, вспомогательная территория, кладбище.
    /// </summary>
    public class PlayerZone
    {
        private readonly Player _player;
        private readonly PlayerCardDeck _desk;
        private readonly PlayerCardDeck _utilityArea;
        private readonly PlayerCardDeck _cemetery;
        private readonly Currency _gold;
        private readonly Currency _silver;

        public PlayerZone(IRules rules, Player player, CardDeck playerDeck)
        {
            _player = player;
            _desk = new PlayerCardDeck(player.Id, playerDeck);
            _utilityArea = new PlayerCardDeck(player.Id, playerDeck);
            _cemetery = new PlayerCardDeck(player.Id, playerDeck);
            _gold = Currency.Gold(rules.GoldAmount);
            _silver = Currency.Silver(rules.SilverAmount);
        }

        public void DealCard(IBaseCard card)
        {
            _desk.Push(card);
        }

        public IBaseCard SelectCard(GameInfo gameInfo)
        {
            var selected = _player.SelectCard(gameInfo);
            var selectesCard = _desk.Pull(selected);
            return selectesCard;
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
