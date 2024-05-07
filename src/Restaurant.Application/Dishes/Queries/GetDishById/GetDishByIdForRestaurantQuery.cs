using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishById;

public class GetDishByIdForRestaurantQuery(int restaurantId,int dishId) : IRequest<DishDto>
{
    public int RestaurantId { get; } = restaurantId;
    public int dishId { get; } = dishId;
}
