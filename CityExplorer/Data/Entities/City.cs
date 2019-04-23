using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityExplorer.Data.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public ICollection<ActivityCity> ActivityCities { get; set; }
    }
}
