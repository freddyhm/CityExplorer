using System.Collections.Generic;

namespace CityExplorer.Data.Entities
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public ICollection<ActivityCity> ActivityCities { get; set; }
        public ICollection<ActivityVenue> ActivityVenues { get; set; }
    }
}