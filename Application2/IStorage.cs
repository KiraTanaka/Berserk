using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application2
{
    public interface IStorage
    {
        IEnumerable<T> FindById<T>(int id);
    }
}
