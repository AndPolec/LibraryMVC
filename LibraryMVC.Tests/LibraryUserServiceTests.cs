using AutoMapper;
using LibraryMVC.Domain.Interfaces;
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

    }
}
