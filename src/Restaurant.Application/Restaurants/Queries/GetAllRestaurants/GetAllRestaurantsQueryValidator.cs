using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private int[] allowPageSizes = [5, 10, 15, 30];
    private string[] allowSortValue = [nameof(RestaurantDto.Name),
                                       nameof(RestaurantDto.Category)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(c => c.PageSize)
            .Must(allowPageSizes.Contains)
            .WithMessage($"Page size must be in [{string.Join(',', allowPageSizes)}]");

        RuleFor(c => c.SortBy)
            .Must(allowSortValue.Contains)
            .When(c=>c.SortBy != null)
            .WithMessage($"Sort by must be empty or in [{string.Join(',', allowSortValue)}]");

        RuleFor(c => c.PageNumber)
            .GreaterThanOrEqualTo(1);
    }
}
