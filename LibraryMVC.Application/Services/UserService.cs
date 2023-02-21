using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.User;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(NewUserVm model)
        {
            throw new NotImplementedException();
        }

        public bool isUserDataExists(string identityUserId)
        {
            return _userRepository.CheckIsUserExistsByIdentityUserId(identityUserId);
        }
    }
}
