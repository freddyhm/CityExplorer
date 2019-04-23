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

        public CitiesController(ICityRepository repository)
        {
            this.repository = repository;
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
        public async Task<ActionResult> Get(string name)
        {
            try
            {
                var results = await repository.GetCityAsync(name);

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
                var existing = await repository.GetCityAsync(city.Name);
                if (existing != null)
                {
                    return BadRequest("City name already in use!");
                }

                var newPath = linkGenerator.GetPathByAction("Get", "Cities",
                    new { name = city.Name });

                if (string.IsNullOrWhiteSpace(newPath))
                {
                    return BadRequest("Could not use city name!");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
