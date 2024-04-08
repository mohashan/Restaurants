using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand:IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assignin user role : {@request}", request);
        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ??throw new NotFoundException<User>(request.UserEmail);
        
        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException<IdentityRole>(request.RoleName);

        await userManager.AddToRoleAsync(user,role.Name!);
    }
}
