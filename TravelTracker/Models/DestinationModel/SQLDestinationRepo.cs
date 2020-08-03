using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelTracker.Models
{
    public class SQLDestinationRepo : IDestinationRepo
    {
        private readonly AppDbContext context;

        public SQLDestinationRepo(AppDbContext context)
        {
            this.context = context;
        }
        public Destination Add(Destination dest)
        {
            context.Destinations.Add(dest);
            context.SaveChanges();
            return dest;
        }

        public Destination Delete(int id)
        {
            var dest = context.Destinations.FirstOrDefault(x => x.Id == id);
            if (dest != null)
            {
                context.Destinations.Remove(dest);
                context.SaveChanges();
            }
            return dest;
        }

        public Destination Get(int id, string user)
        {
            return context.Destinations.FirstOrDefault(x => x.Id == id && x.User == user);
        }

        public IEnumerable<Destination> Get(string user)
        {
            return context.Destinations.Where(x => x.User == user);
        }

        public Destination Update(Destination dest)
        {
            var destination = context.Destinations.Attach(dest);
            destination.State = EntityState.Modified;
            context.SaveChanges();
            return dest;
        }
    }
}
