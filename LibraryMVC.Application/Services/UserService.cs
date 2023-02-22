using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public int AddUser(NewUserVm model)
        {
            var newUser = _mapper.Map<User>(model);
            var newUserId = _userRepository.AddUser(newUser);
            return newUserId;

        }

        public bool isUserDataExists(string identityUserId)
        {
            return _userRepository.CheckIsUserExistsByIdentityUserId(identityUserId);
        }
    }
}
