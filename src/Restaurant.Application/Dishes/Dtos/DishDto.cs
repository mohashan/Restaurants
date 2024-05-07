using Restaurants.Domain.Entities;
using System.Runtime.CompilerServices;

namespace Restaurants.Application.Dishes.Dtos;

public class DishDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int RestaurantId { get; set; }
    public int? KiloCalories { get; set; }
}

