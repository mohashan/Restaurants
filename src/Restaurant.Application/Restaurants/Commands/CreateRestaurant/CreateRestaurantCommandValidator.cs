using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly List<string> categories = ["Sonnati", "Fastfood", "Daryaii", "Hendi"];
    public CreateRestaurantCommandValidator()
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
