using System.Collections.Generic;

namespace CityExplorer.Data.Entities
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; }
        public ICollection<ActivityCity> ActivityCities { get; set; } = new List<ActivityCity> { };
        public ICollection<ActivityVenue> ActivityVenues { get; set; } = new List<ActivityVenue> { };
    }
}