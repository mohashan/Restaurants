using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishRepository
{
    Task<IEnumerable<Dish>> GetAllDishesOfRestaurantAsync(int restaurantId);
    Task<Dish?> GetDishByIdOfRestaurantAsync(int restaurantId,int dishId);
    Task<int> CreateDishAsync(Dish dish);
    Task Delete(IEnumerable<Dish> dishes);
    Task SaveChangesAsync();

}
