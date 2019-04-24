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

        public async Task<Activity[]> GetAllActivitiesAsync()
        {
            IQueryable<Activity> query = context.Activities;

            return await query.ToArrayAsync();
        }

        public async Task<Activity> GetActivityAsync(int id)
        {
            IQueryable<Activity> query = context.Activities.Where(a => a.ActivityId == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Activity> GetActivityAsync(string name)
        {
            IQueryable<Activity> query = context.Activities.Where(a => a.Name == name);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Venue[]> GetAllVenuesAsync()
        {
            IQueryable<Venue> query = context.Venues;

            return await query.ToArrayAsync();
        }

        public async Task<Venue> GetVenueAsync(int id)
        {
            IQueryable<Venue> query = context.Venues.Where(v => v.VenueId == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Venue> GetVenueAsync(string name)
        {
            IQueryable<Venue> query = context.Venues.Where(v => v.Name == name);

            return await query.FirstOrDefaultAsync();
        }
    }
}
