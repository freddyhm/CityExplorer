using CityExplorer.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityExplorer.Data
{
    public interface ICityRepository
    {
        // Cities
        Task<City[]> GetAllCitiesAsync();
        Task<City> GetCityAsync(string name);
    }
}
