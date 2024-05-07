using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantRequirementsHandlerTests
{
    private Mock<ILogger<CreatedMultipleRestaurantRequirementsHandler>> loggerMock;
    private Mock<IRestaurantRepository> restaurantRepositoryMock;
    private CreatedMultipleRestaurantRequirementsHandler? handler;
    private Mock<IUserContext> userContextMock;
    public CreatedMultipleRestaurantRequirementsHandlerTests()
    {
        loggerMock = new Mock<ILogger<CreatedMultipleRestaurantRequirementsHandler>>();
        restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        userContextMock = new Mock<IUserContext>();

    }
    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // Arrange
        var userId = "1";
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser(userId, "test@test.com", [], null, null);
        userContextMock.Setup(c=>c.GetCurrentUser()).Returns(currentUser);

        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = userId
            },
            new()
            {
                OwnerId = userId
            }
        };

        restaurantRepositoryMock.Setup(c=>c.GetAllByOwnerIdAsync(userId)).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantRequirement(2);
        var context = new AuthorizationHandlerContext([requirement],null!,null);

        // Act
        handler = new CreatedMultipleRestaurantRequirementsHandler(loggerMock.Object, restaurantRepositoryMock.Object, userContextMock.Object);

        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldFail()
    {
        // Arrange
        var userId = "1";
        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser(userId, "test@test.com", [], null, null);
        userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);

        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();

        var restaurants = new List<Restaurant>()
        {
            new()
            {
                OwnerId = userId
            },
            new()
            {
                OwnerId = userId
            }
        };

        restaurantRepositoryMock.Setup(c => c.GetAllByOwnerIdAsync(userId)).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantRequirement(3);
        var context = new AuthorizationHandlerContext([requirement], null, null);

        // Act
        handler = new CreatedMultipleRestaurantRequirementsHandler(loggerMock.Object, restaurantRepositoryMock.Object, userContextMock.Object);

        await handler.HandleAsync(context);

        // Assert
        context.HasFailed.Should().BeTrue();
    }
}