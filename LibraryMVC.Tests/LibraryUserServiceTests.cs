using AutoMapper;
using FluentAssertions;
using LibraryMVC.Application.Helpers;
using LibraryMVC.Application.Mapping;
using LibraryMVC.Application.Services;
using LibraryMVC.Application.ViewModels.LibraryUser;
using LibraryMVC.Application.ViewModels.Loan;
using LibraryMVC.Application.ViewModels.User;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Tests
{
    public class LibraryUserServiceTests
    {
        private Mock<ILibraryUserRepository> mockLibraryUserRepo;
        private Mock<IMapper> mockMapper;
        private Mock<IUserTypeRepository> mockUserTypeRepo;
        private Mock<IAdditionalLibrarianInfoRepository> mockAdditionalLibrarianInfoRepo;
        
        public LibraryUserServiceTests()
        {
            SetupMocks();
        }

        private void SetupMocks()
        {
            mockLibraryUserRepo = new Mock<ILibraryUserRepository>();
            mockMapper = new Mock<IMapper>();
            mockUserTypeRepo = new Mock<IUserTypeRepository>();
            mockAdditionalLibrarianInfoRepo = new Mock<IAdditionalLibrarianInfoRepository>();
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
        public void AddUser_ValidModel_ShouldAddUserAndReturnId()
        {
            var libraryUserModel = new NewLibraryUserVm()
            {
                Id = 0,
                FirstName = "Test",
            };
            var libraryUser = new LibraryUser() 
            { 
                Id = 0,
                FirstName = libraryUserModel.FirstName 
            };

            mockMapper.Setup(m => m.Map<LibraryUser>(libraryUserModel)).Returns(libraryUser);
            mockLibraryUserRepo.Setup(r => r.AddUser(libraryUser)).Returns(1);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object,mockUserTypeRepo.Object,mockAdditionalLibrarianInfoRepo.Object);

            var result = service.AddUser(libraryUserModel);

            result.Should().Be(1);
            mockLibraryUserRepo.Verify(r => r.AddUser(libraryUser), Times.Once);
        }
       
        [Fact]
        public void GetAllLibraryUsersForList_WhenCalled_ShouldReturnLibraryUsersListVm()
        {
            var mapper = GetMapper();
            var testLibraryUsersList = new List<LibraryUser>
            {
               new LibraryUser()
               {
                   Id = 1,
                   FirstName = "Jan",
                   LastName = "Kowalski",
                   Loans = new List<Loan>() 
                   { 
                       new Loan() { isOverdue = true, ReturnRecord = new ReturnRecord() { IsPenaltyPaid = false, TotalPenalty = 10  }  }, 
                       new Loan() { isOverdue = true, ReturnRecord = new ReturnRecord() { IsPenaltyPaid = false, TotalPenalty = 10  }  }
                   },
                   Email = "jan@test.pl"
               }
            };

            var expectedLibraryUsersListVm = new List<LibraryUserForListVm>
            {
                new LibraryUserForListVm()
                {
                    Id = 1,
                    FullName = "Jan Kowalski",
                    OverdueLoansCount = 2,
                    UnpaidPenaltiesTotal = 20,
                    Email = "jan@test.pl"
                }
            };

            mockLibraryUserRepo.Setup(r => r.GetAllUsers()).Returns(testLibraryUsersList.AsQueryable());

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mapper, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.GetAllLibraryUsersForList();

            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(expectedLibraryUsersListVm.Count);
            result.Should().BeEquivalentTo(expectedLibraryUsersListVm);
        }

        [Fact]
        public void GetAllLibraryUsersForList_NoUsers_ShouldReturnEmptyList()
        {
            var mapper = GetMapper();
            mockLibraryUserRepo.Setup(r => r.GetAllUsers()).Returns(new List<LibraryUser>().AsQueryable());

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mapper, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.GetAllLibraryUsersForList();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetLibraryUserForDetails_UserFound_ShouldReturnLibraryUserVm()
        {
            var testLibraryUser = new LibraryUser
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski"
            };

            var expectedLibraryUserVm = new LibraryUserDetailsVm()
            {
                Id = 1,
                FullName = "Jan Kowalski"
            };

            mockLibraryUserRepo.Setup(r => r.GetUserById(1)).Returns(testLibraryUser);
            mockMapper.Setup(r => r.Map<LibraryUserDetailsVm>(testLibraryUser)).Returns(expectedLibraryUserVm);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.GetLibraryUserForDetails(1);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedLibraryUserVm);
        }

        [Fact]
        public void GetLibraryUserForDetails_NoUserFound_ShouldThrowNotFoundException()
        {
            var mapper = GetMapper();
            var testId = 1;
            mockLibraryUserRepo.Setup(r => r.GetUserById(1)).Returns((LibraryUser) null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mapper, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Action result = () => service.GetLibraryUserForDetails(testId);

            result.Should().Throw<NotFoundException>().WithMessage($"User with ID {testId} was not found.");
        }

        [Fact]
        public void IsUserDataExists_UserFound_ShouldReturnTrue()
        {
            var userList = new List<LibraryUser>() { new LibraryUser() { IdentityUserId = "test" } };
            mockLibraryUserRepo.Setup(r => r.GetAllUsers()).Returns(userList.AsQueryable);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.IsUserDataExists("test");

            result.Should().BeTrue();
        }

        [Fact]
        public void IsUserDataExists_UserNotFound_ShouldReturnFalse()
        {
            var userList = new List<LibraryUser>();
            mockLibraryUserRepo.Setup(r => r.GetAllUsers()).Returns(userList.AsQueryable);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.IsUserDataExists("test");

            result.Should().BeFalse();
        }

        [Fact]
        public void IsBlocked_UserFoundByIdAndIsNotBlocked_ShouldReturnFalse()
        {
            var userId= 2;
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns(new LibraryUser() { isBlocked = false });

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.IsBlocked(userId);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsBlocked_UserFoundByIdAndIsBlocked_ShouldReturnTrue()
        {
            var userId = 2;
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns(new LibraryUser() { isBlocked = true });

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.IsBlocked(userId);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsBlocked_UserNotFoundById_ShouldThrowNotFoundException()
        {
            var userId = 2;
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns((LibraryUser)null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Action result = () => service.IsBlocked(userId);

            result.Should().Throw<NotFoundException>().WithMessage($"User with ID {userId} was not found.");
        }

        [Fact]
        public void IsBlocked_UserFoundByIdentityIdAndIsNotBlocked_ShouldReturnFalse()
        {
            var identityUserId = "test";
            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(identityUserId)).Returns(new LibraryUser() { isBlocked = false });

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.IsBlocked(identityUserId);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsBlocked_UserFoundByIdentityIdAndIsBlocked_ShouldReturnTrue()
        {
            var identityUserId = "test";
            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(identityUserId)).Returns(new LibraryUser() { isBlocked = true });

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.IsBlocked(identityUserId);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsBlocked_UserNotFoundByIdentityId_ShouldThrowNotFoundException()
        {
            var identityUserId = "test";
            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(identityUserId)).Returns((LibraryUser)null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Action result = () => service.IsBlocked(identityUserId);

            result.Should().Throw<NotFoundException>().WithMessage($"User with identity ID {identityUserId} was not found.");
        }

        [Fact]
        public void BlockUser_UserFoundAndIsNotBlocked_ShouldBlockUserAndReturnTrue()
        {
            var userId = 2;
            var testUser = new LibraryUser() { Id = userId, isBlocked = false };
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns(testUser);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.BlockUser(userId);

            result.Should().BeTrue();
            testUser.isBlocked.Should().BeTrue();
            mockLibraryUserRepo.Verify(r => r.UpdateUser(testUser), Times.Once());
        }

        [Fact]
        public void BlockUser_UserFoundAndIsBlocked_ShouldReturnFalse()
        {
            var userId = 2;
            var testUser = new LibraryUser() { Id = userId, isBlocked = true };
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns(testUser);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.BlockUser(userId);

            result.Should().BeFalse();
            testUser.isBlocked.Should().BeTrue();
            mockLibraryUserRepo.Verify(r => r.UpdateUser(testUser), Times.Never());
        }

        [Fact]
        public void BlockUser_UserNotFound_ShouldThrowNotFoundException()
        {
            var userId = 2;
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns((LibraryUser)null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Action result = () => service.BlockUser(userId);

            result.Should().Throw<NotFoundException>().WithMessage($"User with ID {userId} was not found.");
        }

        [Fact]
        public void UnblockUser_UserFoundAndIsBlocked_ShouldUnblockUserAndReturnTrue()
        {
            var userId = 2;
            var testUser = new LibraryUser() { Id = userId, isBlocked = true };
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns(testUser);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.UnblockUser(userId);

            result.Should().BeTrue();
            testUser.isBlocked.Should().BeFalse();
            mockLibraryUserRepo.Verify(r => r.UpdateUser(testUser), Times.Once());
        }

        [Fact]
        public void UnblockUser_UserFoundAndIsNotBlocked_ShouldReturnFalse()
        {
            var userId = 2;
            var testUser = new LibraryUser() { Id = userId, isBlocked = false };
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns(testUser);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            var result = service.UnblockUser(userId);

            result.Should().BeFalse();
            testUser.isBlocked.Should().BeFalse();
            mockLibraryUserRepo.Verify(r => r.UpdateUser(testUser), Times.Never());
        }

        [Fact]
        public void UnblockUser_UserNotFound_ShouldThrowNotFoundException()
        {
            var userId = 2;
            mockLibraryUserRepo.Setup(r => r.GetUserById(userId)).Returns((LibraryUser)null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Action result = () => service.UnblockUser(userId);

            result.Should().Throw<NotFoundException>().WithMessage($"User with ID {userId} was not found.");
        }

        [Fact]
        public async Task ChangeLibraryUserType_UserNotFound_ShouldThrowNotFoundException()
        {
            var userId = "test";
            var newRoles = new List<string>();

            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(userId)).Returns((LibraryUser)null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Func<Task> result = () => service.ChangeLibraryUserType(userId, newRoles);

            await result.Should().ThrowAsync<NotFoundException>().WithMessage($"User with ID {userId} was not found.");
        }

        [Fact]
        public async Task ChangeLibraryUserType_NewRolesNotFound_ShouldThrowArgumentException()
        {
            var userId = "test";
            var newRoles = new List<string>() {"Role1", "Role2" };

            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(userId)).Returns(new LibraryUser());
            mockUserTypeRepo.Setup(r => r.GetAll()).Returns(new List<UserType>().AsQueryable);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            Func<Task> result = () => service.ChangeLibraryUserType(userId, newRoles);

            await result.Should().ThrowAsync<ArgumentException>().WithMessage($"None of the roles {string.Join(", ", newRoles)} were found.");
        }

        [Fact]
        public async Task ChangeLibraryUserType_ValidInput_ShouldChangeUserType()
        {
            var userId = "test";
            var testUser = new LibraryUser() { IdentityUserId = userId, UserTypes = new List<UserType>() { new UserType() { Name = "Role3" } } };
            var newRoles = new List<string>() { "Role1", "Role2" };
            var newUserTypes = new List<UserType>() { new UserType() { Name = "Role1" }, new UserType() { Name = "Role2" } };

            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(userId)).Returns(testUser);
            mockUserTypeRepo.Setup(r => r.GetAll()).Returns(newUserTypes.AsQueryable);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            await service.ChangeLibraryUserType(userId, newRoles);

            testUser.UserTypes.Should().BeEquivalentTo(newUserTypes);
            mockLibraryUserRepo.Verify(r => r.UpdateUser(testUser), Times.Once);
        }

        [Fact]
        public async Task ChangeLibraryUserType_NewRoleIsLibrarian_ShouldCreateAdditionalLibrarianInfo()
        {
            var userId = "test";
            var testUser = new LibraryUser() { Id = 1 ,IdentityUserId = userId, UserTypes = new List<UserType>() { new UserType() { Name = "Role3" } } };
            var newRoles = new List<string>() { "Bibliotekarz", "Role2" };
            var newUserTypes = new List<UserType>() { new UserType() { Name = "Bibliotekarz" }, new UserType() { Name = "Role2" } };

            mockLibraryUserRepo.Setup(r => r.GetUserByIdentityUserId(userId)).Returns(testUser);
            mockUserTypeRepo.Setup(r => r.GetAll()).Returns(newUserTypes.AsQueryable);
            mockAdditionalLibrarianInfoRepo.Setup(r => r.GetInfoByLibraryUserId(1)).Returns((AdditionalLibrarianInfo)null);

            var service = new LibraryUserService(mockLibraryUserRepo.Object, mockMapper.Object, mockUserTypeRepo.Object, mockAdditionalLibrarianInfoRepo.Object);

            await service.ChangeLibraryUserType(userId, newRoles);

            testUser.UserTypes.Should().BeEquivalentTo(newUserTypes);
            mockLibraryUserRepo.Verify(r => r.UpdateUser(testUser), Times.Once);

            mockAdditionalLibrarianInfoRepo.Verify(
                r => r.AddNewLibrarianInfo(It.Is<AdditionalLibrarianInfo>(info => info.LibraryUserId == testUser.Id)), Times.Once);
        }
    }
}
