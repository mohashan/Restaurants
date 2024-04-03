using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllByRestaurantId;

public class DeleteAllByRestaurantIdCommandHandler(IDishRepository dishRepository,
    IRestaurantRepository restaurantRepository,
    IMapper mapper,
    ILogger<DeleteAllByRestaurantIdCommandHandler> logger) : IRequestHandler<DeleteAllByRestaurantIdCommand>
{
    public async Task Handle(DeleteAllByRestaurantIdCommand request, CancellationToken cancellationToken)
    {
        logger.LogWarning("Deleting All Dishes of Restaurant {restaurantId}",request.RestaurantId);

        var restaurant = await restaurantRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException<Restaurant>(request.RestaurantId.ToString());

        await dishRepository.Delete(restaurant.Dishes);
    }
}