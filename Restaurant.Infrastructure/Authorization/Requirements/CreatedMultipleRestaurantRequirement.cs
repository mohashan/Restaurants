using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class CreatedMultipleRestaurantRequirement(int minimumRestaurantNumber) : IAuthorizationRequirement
{
    public int MinimumRestaurantNumber { get; } = minimumRestaurantNumber;
}


internal class CreatedMultipleRestaurantRequirementsHandler(ILogger<CreatedMultipleRestaurantRequirementsHandler> logger,
    IRestaurantRepository restaurantRepository,
    IUserContext userContext) : AuthorizationHandler<CreatedMultipleRestaurantRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantRequirement requirement)
    {
        logger.LogInformation("Authorization for owner of multiple restaurant : {count}",requirement.MinimumRestaurantNumber);
        var user = userContext.GetCurrentUser();
        var restaurants = await restaurantRepository.GetAllByOwnerIdAsync(user!.Id);
        if (restaurants.Count() >= requirement.MinimumRestaurantNumber)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}