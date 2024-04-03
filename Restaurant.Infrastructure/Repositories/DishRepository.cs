using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishRepository(RestaurantsDbContext dbContext) : IDishRepository
{
    public async Task<int> CreateDishAsync(Dish dish)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();
        return dish.Id;
    }

    public async Task Delete(IEnumerable<Dish> dishes)
    {
        dbContext.Dishes.RemoveRange(dishes);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Dish>> GetAllDishesOfRestaurantAsync(int restaurantId)
    {
        var result = await dbContext.Dishes.Where(c => c.RestaurantId == restaurantId).AsNoTracking().ToListAsync();
        return result;
    }

    public async Task<Dish?> GetDishByIdOfRestaurantAsync(int restaurantId, int dishId)
    {
        var result = await dbContext.Dishes.FirstOrDefaultAsync(c => c.RestaurantId == restaurantId && c.Id == dishId);
        return result;
    }

    public async Task SaveChangesAsync()
    => await dbContext.SaveChangesAsync();
}
