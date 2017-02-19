using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Domain.GameProcess;

namespace Assets.Scripts.Infrastructure
{
    public class DummyStorage
    {
        private readonly List<User> _players;

        public DummyStorage()
        {
            var lines = File.ReadAllLines("DummyStorage.txt"); // temporary code
            _players = new List<User>();
            foreach (var line in lines)
            {
                var splitted = line.Split(',');
                var cardList = new List<int>();
                for (var i = 2; i < splitted.Length; i++)
                {
                    cardList.Add(int.Parse(splitted[i]));
                }
                var player = new User
                {
                    Id = int.Parse(splitted[0]),
                    Name = splitted[1],
                    CardList = cardList
                };
                _players.Add(player);
            }
        }

        public IEnumerable<T> FindById<T>(int id)
        {
            if (typeof(T) == typeof(User))
            {
                return _players.Where(x => x.Id == id).Cast<T>().ToList();
            }

            throw new NotImplementedException();
        }
    }
}

