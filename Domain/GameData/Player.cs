using System;
using Domain.BoardData;
using Domain.CardData;

namespace Domain.GameData
{
    public class Player
    {
        public Guid Id { get; }

        public string Name { get; }

        private readonly IPlayerContext _context;

        public Player(Guid id, string name, IPlayerContext context)
        {
            Id = id;
            Name = name;
            _context = context;
        }

        public IBaseCard SelectCard(GameInfo gameInfo, CardSet cardSet)
        {
            return _context.SelectCard(gameInfo, cardSet, Id);
        }

        public PlayerMove Move(GameInfo gameInfo)
        {
            return _context.Move(gameInfo, Id);
        }

        public PlayerInfo GetInfo()
        {
            return new PlayerInfo
            {
                Id = Id,
                Name = Name
            };
        }

        public override string ToString() => $"{nameof(Player)}: Id={Id}, Name={Name}";
    }
}
