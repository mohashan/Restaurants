using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) :ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var results = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(results);
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int Id)
    {
        var result = await mediator.Send(new GetRestaurantByIdQuery(Id));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantCommand command)
    {
        var id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new {Id= id}, null);
    }

    [HttpDelete("{Id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int Id)
    {
        var result = await mediator.Send(new DeleteRestaurantCommand(Id));

        if (result)
        {
            return NoContent();
        }

            return NotFound();
    }

    [HttpPatch("{Id:int}")]
    public async Task<IActionResult> Patch([FromRoute] int Id, [FromBody] UpdateRestaurantCommand command)
    {
        command.Id = Id;
        var result = await mediator.Send(command);

        if (result)
        {
            return NoContent();
        }

        return NotFound();
    }
}
