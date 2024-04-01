using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators;

public class CreateRestaurantDtoValidator:AbstractValidator<CreateRestaurantDto>
{
    private readonly List<string> categories = ["Sonnati","Fastfood","Daryaii","Hendi"];
    public CreateRestaurantDtoValidator()
    {
        RuleFor(c => c.Name)
            .Length(2, 50);

        RuleFor(c => c.Description)
            .NotEmpty();

        RuleFor(c => c.ContactEmail)
            .EmailAddress();

        RuleFor(c => c.PostalCode)
            .Matches(@"^\d{10}$");

        RuleFor(c => c.Street)
            .NotEmpty()
            .NotNull()
            .When(c => !string.IsNullOrEmpty(c.City));

        RuleFor(c => c.Category)
            .Must(categories.Contains);
        //RuleFor(c => c.Category).Custom((value, context) =>
        //{
        //    var IsValidCategory = categories.Contains(value);
        //    if (!IsValidCategory)
        //        context.AddFailure("Category", "Category is not in the list.");
        //});
    }
}
