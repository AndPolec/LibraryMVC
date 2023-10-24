using AutoMapper;
using FluentAssertions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.Mapping;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.IdentityUserRoles;
using LibraryMVC.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Tests
{
    public class IdentityUserRolesServiceTests
    {
        private Mock<ILibraryUserService> mockLibraryUserService;
        private Mock<IMapper> mockMapper;
        private Mock<UserManager<IdentityUser>> mockUserManager;
        private Mock<RoleManager<IdentityRole>> mockRoleManager;

        public IdentityUserRolesServiceTests()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            mockLibraryUserService = new Mock<ILibraryUserService>();
            mockMapper = new Mock<IMapper>();
            mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<IdentityUser>>().Object,
                new IUserValidator<IdentityUser>[0],
                new IPasswordValidator<IdentityUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<IdentityUser>>>().Object);
            
            mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object,
                new IRoleValidator<IdentityRole>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<IdentityRole>>>().Object);
        }

        private IMapper GetMapper()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new BookProfile());
                config.AddProfile(new GenreProfile());
                config.AddProfile(new AuthorProfile());
                config.AddProfile(new PublisherProfile());
                config.AddProfile(new LoanProfile());
                config.AddProfile(new UserProfile());
            });
            return mapperConfig.CreateMapper();
        }

        [Fact]
        public void GetAllUsers_WhenCall_ShouldReturnListOfIdentityUsers()
        {
            var mapper = GetMapper();
            var usersList = new List<IdentityUser>() { new IdentityUser() { Id = "TestId1", UserName = "User1" }, new IdentityUser() { Id = "TestId2", UserName = "User2" } };
            var expectedUsersListVm = new List<IdentityUsersForListVm>() { new IdentityUsersForListVm() { Id = "TestId1", UserName = "User1" }, new IdentityUsersForListVm() { Id = "TestId2", UserName = "User2" } };

            mockUserManager.Setup(m => m.Users).Returns(usersList.AsQueryable);

            var service = new IdentityUserRolesService(mockUserManager.Object, mockRoleManager.Object,mapper,mockLibraryUserService.Object);

            var result = service.GetAllUsers();

            result.Should().BeEquivalentTo(expectedUsersListVm);
        }

        [Fact]
        public void GetAllRoles_WhenCall_ShouldReturnListOfRoles()
        {
            var mapper = GetMapper();
            var rolesList = new List<IdentityRole>() { new IdentityRole() { Id = "TestId1", Name = "Role1" }, new IdentityRole() { Id = "TestId2", Name = "Role2" } };
            var expectedRolesListVm = new List<RoleVm>() { new RoleVm() { Id = "TestId1", Name = "Role1" }, new RoleVm() { Id = "TestId2", Name = "Role2" } };

            mockRoleManager.Setup(m => m.Roles).Returns(rolesList.AsQueryable);

            var service = new IdentityUserRolesService(mockUserManager.Object, mockRoleManager.Object, mapper, mockLibraryUserService.Object);

            var result = service.GetAllRoles();

            result.Should().BeEquivalentTo(expectedRolesListVm);
        }
    }
}
