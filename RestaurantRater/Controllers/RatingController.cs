using RestaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RatingController : ApiController
    {

        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        //video timestamp 3:03
        // Create New Ratings
        [HttpPost]
        public async Task<IHttpActionResult> CreateRating([FromBody] Rating model)
        {
            // check if model is null
            if (model is null)
                return BadRequest("Your request body cannot be empty.");

            // check if ModelState is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // find the restaurant by model.RestaurantId and see that it exists
            var restaurantEntity = await _context.Restaurants.FindAsync(model.RestaurantId);
            if (restaurantEntity is null)
                return BadRequest($"The target restaurant with an ID of {model.RestaurantId} does not exist.");

            // create the rating

            // add to the rating table
            //_context.Ratings.Add(model);

            // add to the restaurant entity
            restaurantEntity.Ratings.Add(model);
            if (await _context.SaveChangesAsync() == 1)
                return Ok($"You rated restaurant {restaurantEntity.Name} successfully!");

            return InternalServerError();

        }

        // Get a rating by its ID
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingByID([FromUri] int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                return Ok(rating);
            }

            return NotFound();
        }

        // Get All Ratings
        [HttpGet]
        public async Task<IHttpActionResult> GetAllRatings()
        {
            List<Rating> ratings = await _context.Ratings.ToListAsync();
            return Ok(ratings);
        }

        // GET ALL Ratings for a specific restaurant by the Restaurant ID
        //[HttpGet]
        //public async Task<IHttpActionResult> GetAllRatingsByRestaurantID([FromUri] int id, List<Restaurant> restaurantId)
        //{

        //}
        // Update a Rating
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRating([FromUri] int id, [FromBody] Rating updatedRating)
        {
            //check if IDs match
            if(id != updatedRating?.Id)
            {
                return BadRequest("IDs do not match.");
            }

            //check ModelState
            if (ModelState.IsValid)
                return BadRequest(ModelState);

            // find rating in database
            Rating rating = await _context.Ratings.FindAsync(id);

            //if rating does not exist, then do something
            if (rating is null)
                return NotFound();

            //update properties
            rating.FoodScore = updatedRating.FoodScore;
            rating.CleanlinessScore = updatedRating.CleanlinessScore;
            rating.EnvironmentScore = updatedRating.EnvironmentScore;

            //save changes
            await _context.SaveChangesAsync();

            return Ok("The rating was updated!");


        }
        // Delete a Rating
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRating([FromUri] int id)
        {
            Rating rating = await _context.Ratings.FindAsync(id);
            if (rating is null)
                return NotFound();

            _context.Ratings.Remove(rating);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The rating was deleted.");
            }
            return InternalServerError();
        }
    }
}
