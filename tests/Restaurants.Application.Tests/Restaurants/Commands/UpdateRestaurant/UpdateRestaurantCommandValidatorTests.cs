using Xunit;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;
public class UpdateRestaurantCommandValidatorTests
{
    [Fact()]
    public void UpdateRestaurantCommandValidator_ValidRestaurant_ShouldBeValid()
    {
        // Arrange
        var validator = new UpdateRestaurantCommandValidator();
        var restaurant = new UpdateRestaurantCommand
        {
            Description = "description",
            Name = "name",
        };

        // Act
        var validationResult = validator.TestValidate(restaurant);

        // Assert
        validationResult.ShouldNotHaveAnyValidationErrors();
    }

    [Fact()]
    public void UpdateRestaurantCommandValidator_InvalidRestaurant_ShouldHaveValidationError()
    {
        // Arrange
        var validator = new UpdateRestaurantCommandValidator();
        var restaurant = new UpdateRestaurantCommand
        {
            Name = "a",
        };

        // Act
        var validationResult = validator.TestValidate(restaurant);

        // Assert
        validationResult.ShouldHaveValidationErrorFor(c=>c.Name);
        validationResult.ShouldHaveValidationErrorFor(c=>c.Description);
    }
}