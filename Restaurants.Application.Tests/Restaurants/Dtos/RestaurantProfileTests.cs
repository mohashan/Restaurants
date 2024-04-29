using AutoMapper;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Restaurants.Dtos.Tests;

public class RestaurantProfileTests
{
    private readonly IMapper _mapper;
    public RestaurantProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<RestaurantProfile>();
        });

        _mapper = configuration.CreateMapper();
    }


    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        // Arrange
        

        var restaurant = new Restaurant()
        {
            Id = 1,
            Name = "name",
            Category = "category",
            Description = "description",
            HasDelivery = true,
            Address = new Address
            {
                Street = "street",
                PostalCode = "1234567890",
                City = "city"
            }
        };

        // Act
        var dto = _mapper.Map<RestaurantDto>(restaurant);

        //Assert
        dto.Should().NotBeNull();
        dto.Id.Should().Be(restaurant.Id);
        dto.Name.Should().Be(restaurant.Name);
        dto.Description.Should().Be(restaurant.Description);
        dto.Category.Should().Be(restaurant.Category);
        dto.HasDelivery.Should().Be(restaurant.HasDelivery);
        dto.City.Should().Be(restaurant.Address.City);
        dto.Street.Should().Be(restaurant.Address.Street);
        dto.PostalCode.Should().Be(restaurant.Address.PostalCode);
    }


    [Fact()]
    public void CreateMap_ForCreateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange

        var restaurantCommand = new CreateRestaurantCommand()
        {
            Name = "name",
            Category = "category",
            Description = "description",
            HasDelivery = true,
            Street = "street",
            PostalCode = "1234567890",
            City = "city",
            ContactEmail = "test@test.com",
            ContactNumber = "12312312"

        };

        // Act
        var restaurant = _mapper.Map<Restaurant>(restaurantCommand);

        //Assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(restaurantCommand.Name);
        restaurant.Description.Should().Be(restaurantCommand.Description);
        restaurant.Category.Should().Be(restaurantCommand.Category);
        restaurant.HasDelivery.Should().Be(restaurantCommand.HasDelivery);
        restaurant.Address.Should().NotBeNull();
        restaurant.Address.City.Should().Be(restaurantCommand.City);
        restaurant.Address.Street.Should().Be(restaurantCommand.Street);
        restaurant.Address.PostalCode.Should().Be(restaurantCommand.PostalCode);
        restaurant.ContactNumber.Should().Be(restaurantCommand.ContactNumber);
        restaurant.ContactEmail.Should().Be(restaurantCommand.ContactEmail);
    }

    [Fact()]
    public void CreateMap_ForUpdateRestaurantCommandToRestaurant_MapsCorrectly()
    {
        // Arrange
        var restaurantCommand = new UpdateRestaurantCommand()
        {
            Id = 1,
            Name = "name",
            Description = "description",
            HasDelivery = true,
        };

        // Act
        var restaurant = _mapper.Map<Restaurant>(restaurantCommand);

        //Assert
        restaurant.Should().NotBeNull();
        restaurant.Id.Should().Be(restaurantCommand.Id);
        restaurant.Name.Should().Be(restaurantCommand.Name);
        restaurant.Description.Should().Be(restaurantCommand.Description);
        restaurant.HasDelivery.Should().Be(restaurantCommand.HasDelivery);
    }
}