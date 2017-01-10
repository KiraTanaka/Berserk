﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Application.Controllers;
using Domain;

namespace Application.Models
{
    public class Game
    {
        private readonly List<ICard> _cards;
        private readonly IStorage _storage;

        public Game(IEnumerable<ICard> cards, Tuple<int, int> players, IStorage storage)
        {
            _cards = cards.ToList();
            _storage = storage;

            var player1 = storage.FindById<User>(players.Item1).FirstOrDefault();
            var player2 = storage.FindById<User>(players.Item2).FirstOrDefault();

            var deck1 = _cards.Where(x => player1.Cards.Contains(x.Id)).ToList();
            var deck2 = _cards.Where(x => player2.Cards.Contains(x.Id)).ToList();
        }
    }
}