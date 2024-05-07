using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
    IRestaurantRepository restaurantRepository,
    IMapper mapper) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Find Restaurant By Id : {request.Id}");
        var result = await restaurantRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException<Restaurant>(request.Id.ToString());

        return mapper.Map<RestaurantDto>(result);
    }
}

