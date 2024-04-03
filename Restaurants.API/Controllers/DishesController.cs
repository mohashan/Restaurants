using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteAllByRestaurantId;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishById;
using Restaurants.Application.Dishes.Queries.GetDishesOfRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetRestaurantDishes([FromRoute] int restaurantId)
    {
        var result = await mediator.Send(new GetDishesOfRestaurantQuery(restaurantId));
        return Ok(result);
    }

    [HttpGet("{dishId}")]
    public async Task<ActionResult<DishDto>> GetDish([FromRoute] int restaurantId,[FromRoute] int dishId)
    {
        var result = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId,dishId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand dishCommand)
    {
        dishCommand.RestaurantId = restaurantId;
        var dishId = await mediator.Send(dishCommand);
        return CreatedAtAction(nameof(GetDish), new { restaurantId, dishId }, null);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllDishes([FromRoute] int restaurantId)
    {
        await mediator.Send(new DeleteAllByRestaurantIdCommand(restaurantId));
        return NoContent();
    }
}
