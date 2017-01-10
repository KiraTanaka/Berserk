﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Application2
{
    public class StorageMock : IStorage
    {
        private readonly List<User> _players;

        public StorageMock()
        {
            _players = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "User_1",
                    Cards = new[] { 95453, 95455, 87299, 91307, 95453, 95455, 87299, 91307 }
                },
                new User
                {
                    Id = 2,
                    Name = "User_2",
                    Cards = new[] { 95453, 95455, 87299, 91307, 95453, 95455, 87299, 91307 }
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