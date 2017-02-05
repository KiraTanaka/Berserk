using System.Collections.Generic;

namespace Domain.Process
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<int> CardList { get; set; }
    }
}
