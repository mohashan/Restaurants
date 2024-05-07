using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
{
    public int MinimumAge { get; } = minimumAge;
}
