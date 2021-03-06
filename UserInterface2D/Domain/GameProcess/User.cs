﻿using System.Collections.Generic;

namespace Domain.GameProcess
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<int> CardList { get; set; }
    }
}
