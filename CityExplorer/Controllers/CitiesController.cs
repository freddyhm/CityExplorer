using System;
using System.Threading.Tasks;
using CityExplorer.Data;
using CityExplorer.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CityExplorer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository repository;
        private readonly LinkGenerator linkGenerator;

        public CitiesController(ICityRepository repository, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var results = await repository.GetAllCitiesAsync();
                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        [HttpGet("{name}")]
        public async Task<ActionResult> Get(int cityId)
        {
            try
            {
                var results = await repository.GetCityAsync(cityId);

                if (results == null)
                    return NotFound();

                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(City city)
        {
            try
            {
                var existing = await repository.GetCityAsync(city.CityId);
                if (existing != null)
                {
                    return BadRequest("City name already in use!");
                }

                var newPath = linkGenerator.GetPathByAction("Get",
                    "Cities",
                    new { name = city.Name });

                if (string.IsNullOrWhiteSpace(newPath))
                {
                    return BadRequest("Could not use city name!");
                }

                repository.Add(city);

                if (await repository.SaveChangesAsync())
                {
                    var newCity = await repository.GetCityAsync(city.CityId);
                    return Created(newPath, newCity);
                }

                return Ok();
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> Delete(int cityId)
        {
            try
            {
                var oldCity = await repository.GetCityAsync(cityId);
                if (oldCity == null)
                {
                    return NotFound("Could not find the city {name}");
                }

                repository.Delete(oldCity);

                if (await repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to delete the city");
        }

        [HttpPut("{cityId}")]
        public async Task<ActionResult<City>> Put(int cityId, City city)
        {
            try
            {
                var oldCity = await repository.GetCityAsync(cityId);
                if (oldCity == null)
                {
                    return NotFound("Could not find the city {name}");
                }

                oldCity.Name = city.Name;
                oldCity.ActivityCities = city.ActivityCities;

                if (await repository.SaveChangesAsync())
                {
                    return oldCity;
                }
            }
            catch (Exception)
            {         
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }
    }
}
