using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants;

internal class RestaurantService(IRestaurantRepository restaurantRepository, 
    ILogger<RestaurantService> logger,
    IMapper mapper) : IRestaurantService
{
    public async Task<int> CreateRestaurantAsync(CreateRestaurantDto dto)
    {
        var restaurant = mapper.Map<Restaurant>(dto);
        int Id = await restaurantRepository.Create(restaurant);
        return Id;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
    {
        logger.LogInformation("Get All Restaurants");
        var results = await restaurantRepository.GetAllAsync();
        return mapper.Map<IEnumerable<RestaurantDto>>(results);
    }

    public async Task<RestaurantDto?> GetRestaurantByIdAsync(int Id)
    {
        logger.LogInformation("Find Restaurant By Id");
        var result = await restaurantRepository.GetByIdAsync(Id);
        return mapper.Map<RestaurantDto?>(result);
    }
}
