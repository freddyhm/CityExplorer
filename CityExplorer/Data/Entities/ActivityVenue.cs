using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityExplorer.Data.Entities
{
    public class ActivityVenue
    {
        public int ActivityId { get; set; }
        public int VenueId { get; set; }

        public Activity Activity { get; set; }
        public Venue Venue { get; set; }
    }
}
