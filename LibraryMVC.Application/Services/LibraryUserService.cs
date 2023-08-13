using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.LibraryUser;
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
    public class LibraryUserService : ILibraryUserService
    {
        private readonly ILibraryUserRepository _libraryUserRepository;
        private readonly IMapper _mapper;
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IAdditionalLibrarianInfoRepository _additionalLibrarianInfoRepository;

        public LibraryUserService(ILibraryUserRepository userRepository, IMapper mapper, IUserTypeRepository userTypeRepository, IAdditionalLibrarianInfoRepository additionalLibrarianInfoRepository)
        {
            _libraryUserRepository = userRepository;
            _mapper = mapper;
            _userTypeRepository = userTypeRepository;
            _additionalLibrarianInfoRepository = additionalLibrarianInfoRepository;
        }

        public int AddUser(NewLibraryUserVm model)
        {
            var newUser = _mapper.Map<LibraryUser>(model);
            newUser.BorrowingCart = new BorrowingCart();
            var newUserId = _libraryUserRepository.AddUser(newUser);
            return newUserId;
        }

        public List<LibraryUserForListVm> GetAllLibraryUsersForList()
        {
            var users = _libraryUserRepository.GetAllUsers().ProjectTo<LibraryUserForListVm>(_mapper.ConfigurationProvider).ToList();
            return users;
        }

        public LibraryUserDetailsVm GetLibraryUserForDetails(int id)
        {
            var libraryUser = _libraryUserRepository.GetUserById(id);
            var libraryUserVm = _mapper.Map<LibraryUserDetailsVm>(libraryUser);
            return libraryUserVm;
        }

        public bool IsUserDataExists(string identityUserId)
        {
            return _libraryUserRepository.CheckIsUserExistsByIdentityUserId(identityUserId);
        }

        public bool IsBlocked(int userId)
        {
            var user = _libraryUserRepository.GetUserById(userId);
            return user.isBlocked;
        }

        public bool IsBlocked(string identityUserId)
        {
            var user = _libraryUserRepository.GetUserByIdentityUserId(identityUserId);
            return user.isBlocked;
        }

        public bool BlockUser(int userId)
        {
            var user = _libraryUserRepository.GetUserById(userId);
            if (user is null)
            {
                return false;
            }

            user.isBlocked = true;
            _libraryUserRepository.UpdateUser(user);
            return true;
        }

        public bool UnblockUser(int userId)
        {
            var user = _libraryUserRepository.GetUserById(userId);
            if (user is null)
            {
                return false;
            }

            user.isBlocked = false;
            _libraryUserRepository.UpdateUser(user);
            return true;
        }

        public async Task ChangeLibraryUserType(string userId, List<string> newRoles)
        {
            var libraryUser = await Task.Run(() => _libraryUserRepository.GetUserByIdentityUserId(userId));
            var newUserTypes = await Task.Run(() => _userTypeRepository.GetAll().Where(ut => newRoles.Contains(ut.Name)).ToList());
            libraryUser.UserTypes.Clear();
            foreach (var userType in newUserTypes)
            {
                libraryUser.UserTypes.Add(userType);
            }

            if (newRoles.Contains("Bibliotekarz"))
                CreateAdditionalLibrarianInfoForLibraryUser(libraryUser);

            await Task.Run(() => _libraryUserRepository.UpdateUser(libraryUser));
        }

        private async void CreateAdditionalLibrarianInfoForLibraryUser(LibraryUser user)
        {
            if (!IsAdditionalLibrarianInfoExists(user.Id))
            {
                var newLibrarianInfo = new AdditionalLibrarianInfo() { LibraryUserId = user.Id };
                await _additionalLibrarianInfoRepository.AddNewLibrarianInfo(newLibrarianInfo);
            }
        }

        private bool IsAdditionalLibrarianInfoExists(int libraryUserId)
        {
            var additionalLibrarianInfo = _additionalLibrarianInfoRepository.GetInfoByLibraryUserId(libraryUserId);
            return additionalLibrarianInfo == null ? false : true;
        }

        public LibraryUserForPersonalDataVm GetLibraryUserForPersonalData(int id)
        {
            var libraryUser = _libraryUserRepository.GetUserById(id);
            var libraryUserVm = _mapper.Map<LibraryUserForPersonalDataVm>(libraryUser);
            return libraryUserVm;
        }

        public int GetLibraryUserIdByIdentityUserId(string id)
        {
            var user = _libraryUserRepository.GetUserByIdentityUserId(id);
            return user.Id;
        }

        public NewLibraryUserVm GetInfoForUserEdit(int id)
        {
            var user = _libraryUserRepository.GetUserById(id);
            var userVm = _mapper.Map<NewLibraryUserVm>(user);
            return userVm;
        }

        public void UpdateUser(NewLibraryUserVm model)
        {
            var user = _mapper.Map<LibraryUser>(model);
            _libraryUserRepository.UpdateUser(user);
        }
    }
}
