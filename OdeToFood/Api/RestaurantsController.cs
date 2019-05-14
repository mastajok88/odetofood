using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Core;
using OdeToFood.Data;

namespace OdeToFood.Api
{
    [Route("api/[controller]"), ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly OdeToFoodDbContext _context;

        public RestaurantsController(OdeToFoodDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Restaurant> GetRestaurants()
        {
            return _context.Restaurants;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurants([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }
        
        
    }
}