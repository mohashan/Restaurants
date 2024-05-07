using Xunit;
using Restaurants.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Restaurants.Domain.Constants;
using FluentAssertions;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // Arrange
            var dateOfBirth = new DateOnly(1986, 7, 10);
            var nationality = "Iran";
            var httpcontextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier,"1"),
                new (ClaimTypes.Email,"test@test.com"),
                new Claim(ClaimTypes.Role,UserRoles.Admin),
                new Claim(ClaimTypes.Role,UserRoles.User),
                new("Nationality",nationality),
                new("DateOfBirth",dateOfBirth.ToString("yyyy-MM-dd"))
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims,"Test"));

            httpcontextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            var userContext = new UserContext(httpcontextAccessorMock.Object);

            // Act
            var currentUser = userContext.GetCurrentUser();

            // Assert
            currentUser.Should().NotBeNull();
            currentUser.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin,UserRoles.User);
            currentUser.Nationality.Should().Be(nationality);
            currentUser.DateOfBirth.Should().Be(dateOfBirth);


        }

        [Fact]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowInvalidOperationException()
        {
            // Arrange
            var httpcontextAccessorMock = new Mock<IHttpContextAccessor>();

            httpcontextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpcontextAccessorMock.Object);

            // Act
            Action act = ()=> userContext.GetCurrentUser();

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}