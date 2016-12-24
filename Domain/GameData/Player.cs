using System;
using System.Collections.Generic;
using Domain.BoardData;
using Domain.CardData;

namespace Domain.GameData
{
    public class Player
    {
        public Guid Id { get; }

        private readonly Context _context;

        public Player(Context context)
        {
            Id = Guid.NewGuid();
            _context = context;
        }

        public Card SelectCard(List<CardPlace<Card>> cardSet)
        {
            throw new NotImplementedException();
        }
    }
}
