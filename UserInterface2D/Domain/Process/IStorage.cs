using System.Collections.Generic;

namespace Domain.Process
{
    public interface IStorage
    {
        IEnumerable<T> FindById<T>(int id);
    }
}
