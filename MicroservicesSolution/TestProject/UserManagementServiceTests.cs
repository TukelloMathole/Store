using Microsoft.AspNetCore.Identity; 
using Microsoft.Extensions.Logging; 
using Moq; 
using System; 
using System.Threading.Tasks; 
using UserService.Models; 
using UserService.Services; 
using Xunit; 
using Microsoft.EntityFrameworkCore; 
using UserService.Data;
using UserService.DTOs;

namespace TestProject
{
    public class UserManagementServiceTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<ApplicationDbContext> _dbContextMock;
        private readonly Mock<ILogger<UserManagementService>> _loggerMock;
        private readonly UserManagementService _userManagementService;

        public UserManagementServiceTests()
        {
            // Mocking UserStore for UserManager
            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Mocking RoleStore for RoleManager
            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            // Mocking ApplicationDbContext
            var options = new DbContextOptions<ApplicationDbContext>();
            _dbContextMock = new Mock<ApplicationDbContext>(options); 

            // Mocking Logger
            _loggerMock = new Mock<ILogger<UserManagementService>>();

            _userManagementService = new UserManagementService(
                _userManagerMock.Object,
                null, 
                _roleManagerMock.Object,
                _dbContextMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task FindUserByEmailAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var email = "test@example.com";
            var user = new IdentityUser { Email = email };
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userManagementService.FindUserByEmailAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(email, result.Email);
        }

        [Fact]
        public async Task FindUserByEmailAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var email = "nonexistent@example.com";
            _userManagerMock.Setup(um => um.FindByEmailAsync(email)).ReturnsAsync((IdentityUser)null);

            // Act
            var result = await _userManagementService.FindUserByEmailAsync(email);

            // Assert
            Assert.Null(result);
        }

        // testing registration

        [Fact]
        public async Task RegisterUserAsync_ReturnsTrue_WhenUserIsCreatedSuccessfully()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123!" };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock.Setup(rm => rm.RoleExistsAsync("User"))
                            .ReturnsAsync(false);

            // Act
            var result = await _userManagementService.RegisterUserAsync(registerDto);

            // Assert
            Assert.True(result);
            _roleManagerMock.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Once);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFalse_WhenUserCreationFails()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123!" };

            var errors = new List<IdentityError>
    {
        new IdentityError { Description = "Email already exists." }
    };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _userManagementService.RegisterUserAsync(registerDto);

            // Assert
            Assert.False(result);
            _roleManagerMock.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_DoesNotCreateRole_WhenRoleExists()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "Password123!" };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Success);

            _roleManagerMock.Setup(rm => rm.RoleExistsAsync("User"))
                            .ReturnsAsync(true); // Role already exists

            // Act
            var result = await _userManagementService.RegisterUserAsync(registerDto);

            // Assert
            Assert.True(result);
            _roleManagerMock.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFalse_WhenPasswordIsInvalid()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "test@example.com", Password = "short" }; // Invalid password

            var errors = new List<IdentityError>
    {
        new IdentityError { Description = "Password is too short." }
    };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _userManagementService.RegisterUserAsync(registerDto);

            // Assert
            Assert.False(result);
            _roleManagerMock.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_ReturnsFalse_WhenEmailAlreadyExists()
        {
            // Arrange
            var registerDto = new RegisterDto { Email = "existing@example.com", Password = "Password123!" };

            var errors = new List<IdentityError>
    {
        new IdentityError { Description = "Email already exists." }
    };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), registerDto.Password))
                            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _userManagementService.RegisterUserAsync(registerDto);

            // Assert
            Assert.False(result);
            _roleManagerMock.Verify(rm => rm.CreateAsync(It.IsAny<IdentityRole>()), Times.Never);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"), Times.Never);
        }

        [Fact]
        public async Task AssignRoleAsync_ReturnsTrue_WhenUserAlreadyInRole()
        {
            // Arrange
            var user = new IdentityUser { UserName = "test@example.com" };
            string roleName = "Admin";

            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(roleName)).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, roleName)).ReturnsAsync(true);

            // Act
            var result = await _userManagementService.AssignRoleAsync(user, roleName);

            // Assert
            Assert.True(result);
            _userManagerMock.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssignRoleAsync_ReturnsTrue_WhenUserSuccessfullyAddedToRole()
        {
            // Arrange
            var user = new IdentityUser { UserName = "test@example.com" };
            string roleName = "Admin";

            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(roleName)).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, roleName)).ReturnsAsync(false);
            _userManagerMock.Setup(um => um.AddToRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userManagementService.AssignRoleAsync(user, roleName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task AssignRoleAsync_ReturnsFalse_WhenAddingUserToRoleFails()
        {
            // Arrange
            var user = new IdentityUser { UserName = "test@example.com" };
            string roleName = "Admin";

            _roleManagerMock.Setup(rm => rm.RoleExistsAsync(roleName)).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.IsInRoleAsync(user, roleName)).ReturnsAsync(false);

            var errors = new List<IdentityError> { new IdentityError { Description = "Error adding user to role." } };
            _userManagerMock.Setup(um => um.AddToRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));

            // Act
            var result = await _userManagementService.AssignRoleAsync(user, roleName);

            // Assert
            Assert.False(result);
        }



    }
}
