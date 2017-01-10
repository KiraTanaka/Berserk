using System.Collections.Generic;

namespace Application.Models
{
    public interface IStorage
    {
        IEnumerable<T> FindById<T>(int playersItem2);
    }
}