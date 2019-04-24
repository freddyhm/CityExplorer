using CityExplorer.Data;
using CityExplorer.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace CityExplorer.Controllers
{
    [Route("api/cities/{cityName}/activities/{activityName}/venues")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly ICityRepository repository;
        private readonly LinkGenerator linkGenerator;

        public VenuesController(ICityRepository repository, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var results = await repository.GetAllVenuesAsync();
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
                var results = await repository.GetVenueAsync(name);

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
        public async Task<ActionResult<Venue>> Post(string activityName,Venue venue)
        {
            try
            {
                var existingVenue = await repository.GetVenueAsync(venue.Name);
                if (existingVenue != null)
                {
                    return BadRequest("Venue name already in use!");
                }

                repository.Add(venue);

                if (await repository.SaveChangesAsync())
                {
                    var newVenueSaved = await repository.GetVenueAsync(venue.Name);

                    var activity = await repository.GetActivityAsync(activityName);
                    if (activity == null)
                    {
                        return BadRequest("Activity name does not exist!");
                    }

                    ActivityVenue activityVenue = new ActivityVenue()
                    {
                        ActivityId = activity.ActivityId,
                        VenueId = newVenueSaved.VenueId,
                        Activity = activity,
                        Venue = newVenueSaved
                    };

                    activity.ActivityVenues.Add(activityVenue);
                    newVenueSaved.ActivityVenues.Add(activityVenue);

                    if (await repository.SaveChangesAsync())
                    {
                        var url = linkGenerator.GetPathByAction(HttpContext,
                           "Get",
                           values: new {
                               cityName = "mtl",
                               activityName = activity.Name,
                               name = newVenueSaved.Name });

                        return Created(url, newVenueSaved);
                    }
                    else
                    {
                        return BadRequest("Failed to save new venue!");
                    }
                }

                return BadRequest("Failed to save new venue!");
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{VenueId}")]
        public async Task<IActionResult> Delete(int VenueId)
        {
            try
            {
                var oldVenue = await repository.GetVenueAsync(VenueId);
                if (oldVenue == null)
                {
                    return NotFound("Could not find the Venue {name}");
                }

                repository.Delete(oldVenue);

                if (await repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to delete the Venue");
        }

        [HttpPut("{VenueId}")]
        public async Task<ActionResult<Venue>> Put(int VenueId, Venue Venue)
        {
            try
            {
                var oldVenue = await repository.GetVenueAsync(VenueId);
                if (oldVenue == null)
                {
                    return NotFound("Could not find the Venue {name}");
                }

                oldVenue.Name = Venue.Name;
                oldVenue.Type = Venue.Type;
                oldVenue.Address = Venue.Address;
                oldVenue.Affordability = Venue.Affordability;
                oldVenue.ActivityVenues = Venue.ActivityVenues;

                if (await repository.SaveChangesAsync())
                {
                    return oldVenue;
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
