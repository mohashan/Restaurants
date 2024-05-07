using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IRestaurantRepository> restaurantRepositoryMock = new Mock<IRestaurantRepository>();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantRepository),
                                                          _=>restaurantRepositoryMock.Object));
            });
        });
    }

    [Fact()]
    public async void GetAll_ForValidRequest_Returns200Ok()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/Restaurants?PageNumber=1&PageSize=5");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact()]
    public async void GetAll_ForInvalidPageSize_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/Restaurants?PageSize=2");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async void GetAll_ForInvalidPageNumber_Returns400BadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync("/api/Restaurants?PageNumber=-1");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact()]
    public async void GetById_NotExistId_Returns404NotFound()
    {
        // Arrange
        var id = 999999;
        restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync($"/api/Restaurants/{id}");

        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact()]
    public async void GetById_ExistId_Returns200Ok()
    {
        // Arrange
        var id = 999999;
        var restaurant = new Restaurant
        {
            Id = id,
            Name = "Test",
            Description = "TestDesc",
        };
        restaurantRepositoryMock.Setup(m => m.GetByIdAsync(id)).ReturnsAsync(restaurant);
        var client = _factory.CreateClient();

        // Act
        var result = await client.GetAsync($"/api/Restaurants/{id}");
        var response = await result.Content.ReadFromJsonAsync<RestaurantDto>();


        // Assert
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response.Should().NotBeNull();
        response.Id.Should().Be(id);
        response.Name.Should().Be("Test");
        response.Description.Should().Be("TestDesc");
    }
}