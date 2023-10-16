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
        public void BlockUser_UserFoundByIdentityIdAndIsBlocked_ShouldReturnTrue()
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
    }
}
