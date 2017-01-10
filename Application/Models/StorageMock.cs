﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain;

namespace Application.Models
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
                    Cards = new[] { 87299, 91307 }
                },
                new User
                {
                    Id = 2,
                    Cards = new[] { 95453, 95455 }
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