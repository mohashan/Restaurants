using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (!await dbContext.Database.CanConnectAsync())
        {
            return;
        }

        if (dbContext.Restaurants.Any())
        {
            return;
        }

        dbContext.Restaurants.AddRange(GetRestaurants());
        await dbContext.SaveChangesAsync();
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = [
            new()
            {
               Name = "Atawich",
               Address = new(){
                   City = "Tehran",
                   PostalCode = "123123123",
                   Street = "Marzdaran"
               },
               Category = "FastFood",
               ContactEmail = "Ata@wich.com",
               ContactNumber = "123123123",
               Description = "Description",
               Dishes = [
                   new(){
                       Name = "Pizza",
                       Price = 1230000,
                       Description = "Description",
                   },
                   new(){
                       Name = "Sandwich",
                       Price = 1200000,
                       Description = "Description",
                   },
                   ],
            },
            new()
            {
               Name = "Roohi",
               Address = new(){
                   City = "Tehran",
                   PostalCode = "123123123",
                   Street = "SaadatAbad"
               },
               Category = "Sonnati",
               ContactEmail = "Roo@hi.com",
               ContactNumber = "13123123",
               Description = "Description",
               Dishes = [
                   new(){
                       Name = "Kebab",
                       Price = 1231100,
                       Description = "Description",
                   },
                   new(){
                       Name = "Dizi",
                       Price = 1200900,
                       Description = "Description",
                   },
                   ],
            },
            ];

        return restaurants;
    }
}
