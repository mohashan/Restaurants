using MediatR;
using Restaurants.Application.Dishes.Queries.GetDishesOfRestaurant;

namespace Restaurants.Application.Dishes.Commands.DeleteAllByRestaurantId;

public class DeleteAllByRestaurantIdCommand(int restaurantId):IRequest
{
    public int RestaurantId { get; } = restaurantId;
}
