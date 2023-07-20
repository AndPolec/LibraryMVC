using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.IdentityUserRoles;
using LibraryMVC.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMVC.Application.Services
{
    public class IdentityUserRolesService : IIdentityUserRolesService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILibraryUserRepository _libraryUserRepository;
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public IdentityUserRolesService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IUserTypeRepository userTypeRepository, ILibraryUserRepository libraryUserRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _userTypeRepository = userTypeRepository;
            _libraryUserRepository = libraryUserRepository;
        }

        public List<IdentityUsersForListVm> GetAllUsers()
        {
            var model = _userManager.Users.ProjectTo<IdentityUsersForListVm>(_mapper.ConfigurationProvider).ToList();
            return model;
        }

        public IQueryable<RoleVm> GetAllRoles()
        {
            var rolesVm = _roleManager.Roles.ProjectTo<RoleVm>(_mapper.ConfigurationProvider);
            return rolesVm;
        }

        public async Task<List<string>> GetRolesByUserIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<IdentityUserRolesDetailsVm> GetUserRolesDetailsAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userDetailsVm = _mapper.Map<IdentityUserRolesDetailsVm>(user);
            userDetailsVm.UserRoles = await GetRolesByUserIdAsync(id);
            userDetailsVm.AllRoles = GetAllRoles().ToList();
            return userDetailsVm;
        }

        public async Task<IdentityResult> ChangeUserRolesAsync(string userId, List<string> newRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await ChangeLibraryUserType(userId,newRoles);
            return await _userManager.AddToRolesAsync(user, newRoles);
        }

        public async Task<IdentityResult> SetStandardReaderRoleAsync(IdentityUser user)
        {
            var userRole = new List<string>() { "Czytelnik" };
            await ChangeLibraryUserType(user.Id, userRole);
            return await _userManager.AddToRolesAsync(user, userRole);
        }

        private async Task ChangeLibraryUserType(string userId, List<string> newRoles)
        {
            var libraryUser = await Task.Run(() => _libraryUserRepository.GetUserByIdentityUserId(userId));
            var newUserTypes = await Task.Run(() => _userTypeRepository.GetAll().Where(ut => newRoles.Contains(ut.Name)).ToList());
            libraryUser.UserTypes.Clear();
            foreach (var userType in newUserTypes)
            {
                libraryUser.UserTypes.Add(userType);
            }
            await Task.Run(() => _libraryUserRepository.UpdateUser(libraryUser));
        }
    }
}
