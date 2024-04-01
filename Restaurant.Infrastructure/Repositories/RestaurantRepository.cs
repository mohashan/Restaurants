using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantRepository(RestaurantsDbContext dbContext) : IRestaurantRepository
{
    public async Task<int> Create(Restaurant restaurant)
    {
        dbContext.Add(restaurant);
        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        var results = await dbContext.Restaurants.ToListAsync();
        return results;
    }

    public async Task<Restaurant?> GetByIdAsync(int Id)
    {
        var result = await dbContext.Restaurants
            .Include(c=>c.Dishes)
            .FirstOrDefaultAsync(c=>c.Id == Id);
        return result;
    }


}
