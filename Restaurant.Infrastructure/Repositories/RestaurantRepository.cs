using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

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

    public async Task<(IEnumerable<Restaurant>,int)> GetAllAsync(string? searchPhrase, int pageSize, int pageNumber,string? sortBy, SortDirection sortDirection)
    {
        var lowerSearchPhrase = searchPhrase?.ToLower();
        var baseResult = dbContext.Restaurants.
            Where(c => searchPhrase == null || (c.Name.ToLower().Contains(lowerSearchPhrase) || c.Description.ToLower().Contains(lowerSearchPhrase)));
        var ItemCount = await baseResult.CountAsync();
        
        if(sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                {nameof(Restaurant.Name), x => x.Name},
                {nameof(Restaurant.Category), x => x.Category}
            };

            var selectedColumn = columnsSelector[sortBy];
            if (sortDirection == SortDirection.Descending)
            {
                baseResult = baseResult.OrderByDescending(selectedColumn);
            }
            else
            {
                baseResult = baseResult.OrderBy(selectedColumn);
            }

        }

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
