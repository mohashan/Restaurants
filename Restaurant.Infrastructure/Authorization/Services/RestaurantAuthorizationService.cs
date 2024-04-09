using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;

public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger,
    IUserContext userContext) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
    {
        var user = userContext.GetCurrentUser();
        logger.LogInformation("Authorizing user {UserEmail}, to {Operation} for {Restaurant}",
            user.Email,
            resourceOperation,
            restaurant.Name);

        if (resourceOperation == ResourceOperation.Read)
        {
            logger.LogInformation("Create/Read operation - successfull authorization");
            return true;
        }

        if (resourceOperation == ResourceOperation.Delete && user.IsInRole("Admin"))
        {
            logger.LogInformation("Admin User, Delete operation - successfull authorization");
            return true;
        }

        if ((resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Delete) &&
            user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("Owner User, Delete and update operation - successfull authorization");
            return true;
        }

        return false;
    }


}
