using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantRepository(RestaurantsDbContext dbContext) : IRestaurantRepository
{
    public async Task<int> CreateAsync(Restaurant restaurant)
    {
        dbContext.Add(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant)
    {
        dbContext.Remove(restaurant);
        await dbContext.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Restaurant>,int)> GetAllAsync(string? searchPhrase, int pageSize, int pageNumber)
    {
        var lowerSearchPhrase = searchPhrase?.ToLower();
        var baseResult = dbContext.Restaurants.
            Where(c => searchPhrase == null || (c.Name.ToLower().Contains(lowerSearchPhrase) || c.Description.ToLower().Contains(lowerSearchPhrase)));
        var ItemCount = await baseResult.CountAsync();
        
        var result = await baseResult.
            AsNoTracking().
            Skip((pageNumber - 1) * pageSize).
            Take(pageSize).
            ToListAsync();
        return (result, ItemCount);
    }

    public async Task<IEnumerable<Restaurant>> GetAllByOwnerIdAsync(string OwnerId)
    {
        var restaurants = await dbContext.Restaurants.Where(c => c.OwnerId == OwnerId).ToListAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetByIdAsync(int Id)
    {
        var result = await dbContext.Restaurants
            .Include(c=>c.Dishes)
            .FirstOrDefaultAsync(c=>c.Id == Id);
        return result;
    }

    public async Task SaveChangesAsync()
    => await dbContext.SaveChangesAsync();
}
