using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelTracker.Models
{
    public interface IDestinationRepo
    {
        Destination Get(int id, string user);
        IEnumerable<Destination> Get(string user);
        Destination Add(Destination dest);
        Destination Update(Destination dest);
        Destination Delete(int id);
    }
}
