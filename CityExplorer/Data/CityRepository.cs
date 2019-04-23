using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityExplorer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityExplorer.Data
{
    public class CityRepository : ICityRepository
    {
        private readonly CityContext context;

        public CityRepository(CityContext context)
        {
            this.context = context;
        }

        public async Task<City[]> GetAllCitiesAsync()
        {
            IQueryable<City> query = context.Cities;

            return await query.ToArrayAsync();
        }

        public async Task<City> GetCityAsync(string name)
        {
            IQueryable<City> query = context.Cities.Where(c => c.Name == name);
            return await query.FirstOrDefaultAsync();
        }
    }
}
