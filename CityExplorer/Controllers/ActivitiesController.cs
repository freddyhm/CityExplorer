using CityExplorer.Data;
using CityExplorer.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace CityExplorer.Controllers
{
    [Route("api/cities/{activityName}/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ICityRepository repository;
        private readonly LinkGenerator linkGenerator;

        public ActivitiesController(ICityRepository repository, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var results = await repository.GetAllActivitiesAsync();
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
                var results = await repository.GetActivityAsync(name);

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
        public async Task<ActionResult> Post(Activity Activity)
        {
            try
            {
                var existing = await repository.GetActivityAsync(Activity.ActivityId);
                if (existing != null)
                {
                    return BadRequest("Activity name already in use!");
                }

                var newPath = linkGenerator.GetPathByAction("Get",
                    "Activities",
                    new { name = Activity.Name });

                if (string.IsNullOrWhiteSpace(newPath))
                {
                    return BadRequest("Could not use activity name!");
                }

                repository.Add(Activity);

                if (await repository.SaveChangesAsync())
                {
                    var newActivity = await repository.GetActivityAsync(Activity.ActivityId);
                    return Created(newPath, newActivity);
                }

                return Ok();
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{ActivityId}")]
        public async Task<IActionResult> Delete(int ActivityId)
        {
            try
            {
                var oldActivity = await repository.GetActivityAsync(ActivityId);
                if (oldActivity == null)
                {
                    return NotFound("Could not find the activity {name}");
                }

                repository.Delete(oldActivity);

                if (await repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to delete the activity");
        }

        [HttpPut("{ActivityId}")]
        public async Task<ActionResult<Activity>> Put(int ActivityId, Activity Activity)
        {
            try
            {
                var oldActivity = await repository.GetActivityAsync(ActivityId);
                if (oldActivity == null)
                {
                    return NotFound("Could not find the activity {name}");
                }

                oldActivity.Name = Activity.Name;
                oldActivity.ActivityCities = Activity.ActivityCities;

                if (await repository.SaveChangesAsync())
                {
                    return oldActivity;
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
