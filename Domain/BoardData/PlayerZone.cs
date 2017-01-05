using Domain.CardData;
using Domain.GameData;

namespace Domain.BoardData
{
    /// <summary>
    /// Представляет собой набор данных связанных с игроком.
    /// Игрок, колода, кладбище.
    /// </summary>
    public class PlayerZone
    {
        private readonly Player _player;
        private readonly CardDeck _desk;
        private readonly CardDeck _cemetery;
        private int _currency;

        public PlayerZone(IRules rules, Player player, CardDeck playerDeck)
        {
            _player = player;
            _desk = playerDeck;
            _cemetery = playerDeck;
            _currency = rules.CurrencyAmount;
        }

        public void DealCard(ICard card)
        {
            //_desk.Push(card);
        }

        public ICard SelectCard(GameInfo gameInfo)
        {
            var selected = _player.SelectCard(gameInfo);
            var selectesCard = _desk.Pull(selected);
            return selectesCard;
        }

        public PlayerMove Move(GameInfo gameInfo)
        {
            return _player.Move(gameInfo);
        }

        public PlayerZoneInfo GetInfo()
        {
            return new PlayerZoneInfo
            {
                PlayerId = _player.Id,
                PlayerName = _player.Name,
                Desk = _desk.GetInfo(),
                Cementery = _cemetery.GetInfo(),
                Currency = _currency
            };
        }
    }
}
