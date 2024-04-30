using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System.Security.AccessControl;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTest
{
    private Mock<ILogger<UpdateRestaurantCommandHandler>> loggerMock;
    private Mock<IMapper> mapperMock;
    private Mock<IRestaurantRepository> restaurantRepositoryMock;
    private Mock<IRestaurantAuthorizationService> authorizationServiceMock;
    private UpdateRestaurantCommandHandler handler;
    public UpdateRestaurantCommandHandlerTest()
    {
        loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        mapperMock = new Mock<IMapper>();
        restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        authorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        handler = new UpdateRestaurantCommandHandler(loggerMock.Object, mapperMock.Object, authorizationServiceMock.Object, restaurantRepositoryMock.Object);

    }
    [Fact()]
    public async Task Handle_ForValidCommand_ShouldUpdateRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        restaurantRepositoryMock.Setup(c => c.SaveChangesAsync());
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId,
            Description = "new description",
            HasDelivery = true,
            Name = "new name",
        };
        var userContextMock = new Mock<IUserContext>();

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            Name = "name",
            Description = "description",
        };

        restaurantRepositoryMock.Setup(c => c.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);

        authorizationServiceMock.Setup(c => c.Authorize(restaurant, Domain.Constants.ResourceOperation.Update))
            .Returns(true);



        mapperMock.Setup(c => c.Map(command,restaurant)).Returns(restaurant);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        restaurantRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

        mapperMock.Verify(m=>m.Map(command,restaurant),Times.Once);
    }

    [Fact()]
    public async Task Handle_ForNotExistRestaurant_ShouldThrowNotFound()
    {
        // Arrangec
        var restaurantId = 1;

        restaurantRepositoryMock.Setup(c => c.SaveChangesAsync());
        var command = new UpdateRestaurantCommand()
        {
            Id = 1
        };

        var restaurant = new Restaurant();

        restaurantRepositoryMock.Setup(c => c.GetByIdAsync(restaurantId)).ReturnsAsync((Restaurant?)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException<Restaurant>>()
            .WithMessage($"Restaurant with Id : {restaurantId} is not exist");


    }

    [Fact()]
    public async Task Handle_ForNotExistRestaurant_ShouldThrowForbiden()
    {
        // Arrangec
        var restaurantId = 1;

        restaurantRepositoryMock.Setup(c => c.SaveChangesAsync());
        var command = new UpdateRestaurantCommand()
        {
            Id = restaurantId
        };

        var restaurant = new Restaurant
        {
            Id = restaurantId,
            OwnerId = "owner_id"
        };

        var userContextMock = new Mock<IUserContext>();

        var currentUser = new CurrentUser("owner_id_", "test@test.com", [], null, null);
        userContextMock.Setup(c => c.GetCurrentUser()).Returns(currentUser);

        restaurantRepositoryMock.Setup(c => c.GetByIdAsync(restaurantId)).ReturnsAsync(restaurant);


        authorizationServiceMock.Setup(c => c.Authorize(It.IsAny<Restaurant>(), Domain.Constants.ResourceOperation.Update))
            .Returns(false);



        mapperMock.Setup(c => c.Map(command, restaurant)).Returns(restaurant);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbidException<Restaurant>>()
            .WithMessage($"Restaurant with Id : {restaurantId} is not accessible for you");


    }

}
