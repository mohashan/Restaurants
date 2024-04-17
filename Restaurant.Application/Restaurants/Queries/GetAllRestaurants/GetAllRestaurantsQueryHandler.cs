using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(ILogger<GetAllRestaurantsQueryHandler> logger,
    IRestaurantRepository restaurantRepository,
    IMapper mapper) : IRequestHandler<GetAllRestaurantsQuery, PageResult<RestaurantDto>>
{
    public async Task<PageResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Get All Restaurants");
        var (restaurants,itemCount) = await restaurantRepository.GetAllAsync(request.SearchPhrase,request.PageSize,request.PageNumber);
        
        var restaurantDto = mapper.Map<List<RestaurantDto>>(restaurants);

        var result = new PageResult<RestaurantDto>(restaurantDto,itemCount,request.PageSize,request.PageNumber);
        return result;
    }
}
