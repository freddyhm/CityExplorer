using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityExplorer.Data.Entities
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public int Affordability { get; set; }
        public ICollection<ActivityVenue> ActivityVenues { get; set; }
    }
}
