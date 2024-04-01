using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IRestaurantService restaurantService) :ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await restaurantService.GetAllRestaurantsAsync();
        return Ok(results);
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int Id)
    {
        var result = await restaurantService.GetRestaurantByIdAsync(Id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto restaurantDto)
    {
        var result = await restaurantService.CreateRestaurantAsync(restaurantDto);

        return CreatedAtAction(nameof(GetById), new {Id= result}, null);
    }
}
