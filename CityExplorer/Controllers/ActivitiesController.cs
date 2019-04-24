using CityExplorer.Data;
using CityExplorer.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace CityExplorer.Controllers
{
    [Route("api/[controller]")]
    [Route("api/cities/{cityName}/[controller]")]
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
        public async Task<ActionResult> Post(Activity activity, string cityName = null)
        {
            try
            {
                var existing = await repository.GetActivityAsync(activity.Name);
                if (existing != null)
                {
                    return BadRequest("Activity name already in use!");
                }

                repository.Add(activity);

                if (cityName == null)
                {
                    var newPath = linkGenerator.GetPathByAction("Get",
                    "Activities",
                    new { name = activity.Name });

                    if (string.IsNullOrWhiteSpace(newPath))
                    {
                        return BadRequest("Could not use activity name!");
                    }

                    if (await repository.SaveChangesAsync())
                    {
                        var newActivity = await repository.GetActivityAsync(activity.ActivityId);
                        return Created(newPath, newActivity);
                    }
                    else
                    {
                        return BadRequest("Failed to save new venue!");
                    }
                }
                else
                {
                    if (await repository.SaveChangesAsync())
                    {
                        var newActivitySaved = await repository.GetActivityAsync(activity.Name);

                        var city = await repository.GetCityAsync(cityName);
                        if (city == null)
                        {
                            return BadRequest("City name does not exist!");
                        }

                        ActivityCity activityCity = new ActivityCity()
                        {
                            ActivityId = newActivitySaved.ActivityId,
                            CityId = city.CityId,
                            Activity = newActivitySaved,
                            City = city
                        };

                        activity.ActivityCities.Add(activityCity);
                        newActivitySaved.ActivityCities.Add(activityCity);

                        if (await repository.SaveChangesAsync())
                        {
                            var url = linkGenerator.GetPathByAction(HttpContext,
                               "Get",
                               values: new
                               {
                                   cityName = city.Name,
                                   name = newActivitySaved.Name,
                               });

                            return Created(url, newActivitySaved);
                        }
                        else
                        {
                            return BadRequest("Failed to save new activity!");
                        }
                    }
                    else
                    {
                        return BadRequest("Failed to save new activity!");
                    }
                }
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
