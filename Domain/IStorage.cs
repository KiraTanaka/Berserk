using System.Collections.Generic;

namespace Domain
{
    public interface IStorage
    {
        IEnumerable<T> FindById<T>(int id);
    }
}
