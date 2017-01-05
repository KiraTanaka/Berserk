using System;

namespace Domain.GameData
{
    /// <summary>
    /// Пользовтаель участвующий в игре.
    /// </summary>
    public class Player
    {
        public Guid Id { get; }
        public string Name { get; }
        private readonly IContext _context;

        public Player(Guid id, string name, IContext context)
        {
            Id = id;
            Name = name;
            _context = context;
        }

        public Guid SelectCard(GameInfo gameInfo)
        {
            return _context.SelectCard(Id, gameInfo);
        }

        public PlayerMove Move(GameInfo gameInfo)
        {
            return _context.Move(Id, gameInfo);
        }

        public override string ToString() 
            => $"{nameof(Player)}: Id={Id}, Name={Name}";
    }
}
