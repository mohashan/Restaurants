using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllByRestaurantId;

public class DeleteAllByRestaurantIdCommandHandler(IDishRepository dishRepository,
    IRestaurantRepository restaurantRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    ILogger<DeleteAllByRestaurantIdCommandHandler> logger) : IRequestHandler<DeleteAllByRestaurantIdCommand>
{
    public async Task Handle(DeleteAllByRestaurantIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting All Dishes of Restaurant {restaurantId}",request.RestaurantId);

        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException<Restaurant>(request.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            throw new ForbidException<Restaurant>(restaurant.Id.ToString());

        await dishRepository.Delete(restaurant.Dishes);
    }
}