using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ErrorHandlingMiddleware>> logger;
    public ErrorHandlingMiddlewareTests()
    {
        logger = new Mock<ILogger<ErrorHandlingMiddleware>>();
    }
    [Fact()]
    public async void InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        // Arrange
        var middleware = new ErrorHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        // Act

        await middleware.InvokeAsync(context, nextDelegateMock.Object);

        // Assert
        nextDelegateMock.Verify(c=>c.Invoke(context), Times.Once);

    }

    [Fact()]
    public async void InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404AndReturnException()
    {
        // Arrange
        var middleware = new ErrorHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        var rd = new RequestDelegate((HttpContext c) => { throw new NotFoundException("TestResource","TestId"); });

        // Act

        await middleware.InvokeAsync(context, rd);

        // Assert
        context.Response.StatusCode.Should().Be(404);

    }

    [Fact()]
    public async void InvokeAsync_WhenForbidenExceptionThrown_ShouldSetStatusCode403AndReturnException()
    {
        // Arrange
        var middleware = new ErrorHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        var rd = new RequestDelegate((HttpContext c) => { throw new ForbidException("TestResource", "TestId"); });

        // Act

        await middleware.InvokeAsync(context, rd);

        // Assert
        context.Response.StatusCode.Should().Be(403);

    }
    [Fact()]
    public async void InvokeAsync_WhenForbidenExceptionThrown_ShouldSetStatusCode500()
    {
        // Arrange
        var middleware = new ErrorHandlingMiddleware(logger.Object);
        var context = new DefaultHttpContext();
        var nextDelegateMock = new Mock<RequestDelegate>();

        var rd = new RequestDelegate((HttpContext c) => { throw new Exception("TestResource"); });

        // Act

        await middleware.InvokeAsync(context, rd);

        // Assert
        context.Response.StatusCode.Should().Be(500);

    }

     
}