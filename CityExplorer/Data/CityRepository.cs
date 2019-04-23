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

        public async Task<City> GetCityAsync(int cityId)
        {
            IQueryable<City> query = context.Cities.Where(c => c.CityId == cityId);
            return await query.FirstOrDefaultAsync();
        }

        public void Add<T>(T entity) where T : class
        {
            context.Add(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await context.SaveChangesAsync()) > 0;
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }
    }
}
