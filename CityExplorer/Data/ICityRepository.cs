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
        // General
        void Add<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
        void Delete<T>(T entity) where T : class;

        // Cities
        Task<City[]> GetAllCitiesAsync();
        Task<City> GetCityAsync(int CityId);
        Task<City> GetCityAsync(string cityName);

        // Activities
        Task<Activity[]> GetAllActivitiesAsync();
        Task<Activity> GetActivityAsync(string name);
        Task<Activity> GetActivityAsync(int id);

        // Venues
        Task<Venue[]> GetAllVenuesAsync();
        Task<Venue> GetVenueAsync(string name);
        Task<Venue> GetVenueAsync(int id);

    }
}
