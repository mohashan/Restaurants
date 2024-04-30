using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnCreatedRestaurantId()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        var userContextMock = new Mock<IUserContext>();
        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();

        restaurantRepositoryMock.Setup(c => c.CreateAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(1);

        var currentUser = new CurrentUser("owner_id","test@test.com", [], null,null);
        userContextMock.Setup(c=>c.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object,mapperMock.Object,restaurantRepositoryMock.Object,userContextMock.Object);
        var command = new CreateRestaurantCommand();

        var restaurant = new Restaurant();

        mapperMock.Setup(c => c.Map<Restaurant>(command)).Returns(restaurant);

        // Act
        var result = await commandHandler.Handle(command,CancellationToken.None);

        // Assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be("owner_id");
        restaurantRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once);


    }
}