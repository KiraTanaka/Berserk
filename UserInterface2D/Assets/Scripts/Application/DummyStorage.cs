using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Process;

namespace Assets.Scripts.Application
{
    public class DummyStorage : IStorage
    {
        private readonly List<User> _players;

        public DummyStorage()
        {
            _players = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "User_1",
                    CardList = new[] { 87242, 92314, 87065, 95380, 87693 }
                },
                new User
                {
                    Id = 2,
                    Name = "User_2",
                    CardList = new[] { 87242, 92314, 87065, 95380, 87689 }
                }
            };
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

