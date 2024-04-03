using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishesOfRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishById;

public class GetDishByIdForRestaurantQueryHandler(
    IRestaurantRepository restaurantRepository,
    IMapper mapper,
    ILogger<GetDishByIdForRestaurantQueryHandler> logger) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
{
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException<Restaurant>(request.RestaurantId.ToString());

        var dish = restaurant.Dishes.FirstOrDefault(c=>c.Id == request.dishId)
            ?? throw new NotFoundException<Dish>(request.dishId.ToString());

        return mapper.Map<DishDto>(dish);

    }
}