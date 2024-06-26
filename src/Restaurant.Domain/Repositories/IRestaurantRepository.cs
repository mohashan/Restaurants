﻿using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<(IEnumerable<Restaurant>,int)> GetAllAsync(string? searchPhrase,int pageSize,int pageNumber,string? sortby, SortDirection sortDirection);
    Task<IEnumerable<Restaurant>> GetAllByOwnerIdAsync(string OwnerId);
    Task<Restaurant?> GetByIdAsync(int Id);

    Task<int> CreateAsync(Restaurant restaurant);

    Task DeleteAsync(Restaurant restaurant);
    Task SaveChangesAsync();


}