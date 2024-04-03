using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishesOfRestaurant;

public class GetDishesOfRestaurantQueryHandler(IRestaurantRepository restaurantRepository,
    IMapper mapper,
    ILogger<GetDishesOfRestaurantQueryHandler> logger) : IRequestHandler<GetDishesOfRestaurantQuery, IEnumerable<DishDto>>
{
    public async Task<IEnumerable<DishDto>> Handle(GetDishesOfRestaurantQuery request, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException<Restaurant>(request.RestaurantId.ToString());

        var dishDtos = mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
        return dishDtos;
    }
}
