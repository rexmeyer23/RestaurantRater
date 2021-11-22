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
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        // POST (create)
        // api/Restaurant
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurant([FromBody] Restaurant model)
        {
            if(model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }
            // if the model is valid
            if(ModelState.IsValid)
            {
                // store the model in the database
                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();

                return Ok("Your restaurant was created");
            }

            //the model is not valid, go ahead and reject it
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Restaurant
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        // GET BY ID
        // api/Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if(restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }

        // PUT (update)
        // api/Restaurant/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            // Check the ids if they match
            if(id != updatedRestaurant?.Id) //question mark is asking if updated restaurant model is null, do not access id and will evaluate model as false
            {
                return BadRequest("Ids do not match.");
            }

            // Check the ModelState
            if (!ModelState.IsValid) // curly braces are not needed for single line code
                return BadRequest(ModelState);


            // Find the restaurant in the database
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            // If the restaurant does not exist then do something
            if (restaurant is null)
                return NotFound();

            // Update the properties
            restaurant.Name = updatedRestaurant.Name;
            restaurant.RestaurantType = updatedRestaurant.RestaurantType;
            restaurant.Address = updatedRestaurant.Address;
        
            // Save the changes
            await _context.SaveChangesAsync();

            return Ok("The restaurant was updated!");

        }

        // DELETE (delete)
        // api/Restaurants/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant is null)
                return NotFound();

            _context.Restaurants.Remove(restaurant);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The restaurant was deleted.");
            }

            return InternalServerError();
        }
    }
}
