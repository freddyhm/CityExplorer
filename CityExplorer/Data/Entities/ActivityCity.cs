using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityExplorer.Data.Entities
{
    public class ActivityCity
    {
        public int ActivityId { get; set; }
        public int CityId { get; set; }

        public Activity Activity { get; set; }
        public City City { get; set; }
    }
}
