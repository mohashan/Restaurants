using FluentAssertions;
using Restaurants.Domain.Constants;
using Xunit;
namespace Restaurants.Application.Users.Tests
{
    public class CurrentUserTests
    {
        // TestMethod_Scenario_ExpectResult
        [Theory()]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.User)]
        public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
        {
            // Arrange
            var user = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // Act
            var isInRole = user.IsInRole(UserRoles.Admin.ToLower());

            // Assert
            isInRole.Should().BeFalse();
        }

        [Fact()]
        public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
        {
            // Arrange
            var user = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // Act
            var isInRole = user.IsInRole(UserRoles.Owner);

            // Assert
            isInRole.Should().BeFalse();
        }


        [Fact()]
        public void IsInRole_WithNoMatchingRoleCase_ShouldReturnFalse()
        {
            // Arrange
            var user = new CurrentUser("1", "test@test.com", [UserRoles.Admin, UserRoles.User], null, null);

            // Act
            var isInRole = user.IsInRole(UserRoles.Admin.ToLower());

            // Assert
            isInRole.Should().BeFalse();
        }

        
    }
}