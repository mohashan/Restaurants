using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Queries.GetDishesOfRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandHandler(IDishRepository dishRepository,
    IRestaurantRepository restaurantRepository,
    IMapper mapper,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    ILogger<CreateDishCommandHandler> logger) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException<Restaurant>(request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            throw new ForbidException<Restaurant>(request.RestaurantId.ToString());

        var dish = mapper.Map<Dish>(request);
        var id = await dishRepository.CreateDishAsync(dish);
        return id;
    }
}
