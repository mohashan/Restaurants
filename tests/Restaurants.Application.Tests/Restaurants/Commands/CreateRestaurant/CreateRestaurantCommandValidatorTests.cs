using FluentValidation.TestHelper;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandValidatorTests
{
    [Fact()]
    public void CreateRestaurantCommandValidator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "name",
            Description = "description",
            PostalCode = "0123456789",
            City = "city",
            Street = "street",
            Category = "Sonnati",
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);


        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void CreateRestaurantCommandValidator_ForInvalidCommand_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Name = "n",
            //Description = "description",
            PostalCode = "a0123456789",
            City = "city",
            //Street = "street",
            ContactEmail = "@test.com",
            Category = "asdasdasd",
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);


        // Assert
        result.ShouldHaveValidationErrorFor(c=>c.Name);
        result.ShouldHaveValidationErrorFor(c=>c.Description);
        result.ShouldHaveValidationErrorFor(c=>c.PostalCode);
        result.ShouldHaveValidationErrorFor(c=>c.Street);
        result.ShouldHaveValidationErrorFor(c=>c.ContactEmail);
        result.ShouldHaveValidationErrorFor(c=>c.Category);
    }


    [Theory()]
    [InlineData("Sonnati")]
    [InlineData("Fastfood")]
    [InlineData("Daryaii")]
    [InlineData("Hendi")]
    public void CreateRestaurantCommandValidator_ForValidCategory_ShouldNotHaveValidationErrorsForCategoryProperty(string category)
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            Category = category,
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);


        // Assert
        result.ShouldNotHaveValidationErrorFor(c=>c.Category);
    }


    [Theory()]
    [InlineData("0123-456-789")]
    [InlineData("Fastfood")]
    [InlineData("1234564")]
    [InlineData("123456789012")]
    [InlineData("12345 1200")]
    [InlineData("01234-6789")]
    [InlineData("01234µ6789")]
    public void CreateRestaurantCommandValidator_ForInvalidPostalCode_ShouldHaveValidationErrorForPostalCodeProperty(string postalCode)
    {
        // Arrange
        var command = new CreateRestaurantCommand()
        {
            PostalCode = postalCode,
        };

        var validator = new CreateRestaurantCommandValidator();

        // Act
        var result = validator.TestValidate(command);


        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }
}