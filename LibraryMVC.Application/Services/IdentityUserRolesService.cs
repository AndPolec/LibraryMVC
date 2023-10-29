using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Helpers;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.IdentityUserRoles;
using LibraryMVC.Domain.Interfaces;
using LibraryMVC.Domain.Model;
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
        private readonly IMapper _mapper;
        private readonly ILibraryUserService _libraryUserService;

        public IdentityUserRolesService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ILibraryUserService libraryUserService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _libraryUserService = libraryUserService;
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
            if (user == null)
            {
                throw new NotFoundException($"No identityUser found for user with ID: {id}");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<IdentityUserRolesDetailsVm> GetUserRolesDetailsAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"No identityUser found for user with ID: {id}");
            }

            var userDetailsVm = _mapper.Map<IdentityUserRolesDetailsVm>(user);
            userDetailsVm.UserRoles = await GetRolesByUserIdAsync(id);
            userDetailsVm.AllRoles = GetAllRoles().ToList();
            return userDetailsVm;
        }

        public async Task ChangeUserRolesAsync(string userId, List<string> newRoles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"No identityUser found for user with ID: {userId}");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (!IsRoleChanged(userRoles, newRoles))
            {
                return;
            }

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _libraryUserService.ChangeLibraryUserType(userId,newRoles);
            await _userManager.AddToRolesAsync(user, newRoles);
            return;
        }

        private bool IsRoleChanged(IList<string> userRoles, List<string> newRoles)
        {
            if (newRoles.All(r => userRoles.Contains(r)) && newRoles.Count == userRoles.Count)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<IdentityResult> SetStandardReaderRoleAsync(IdentityUser user)
        {
            var userRole = new List<string>() { "Czytelnik" };
            await _libraryUserService.ChangeLibraryUserType(user.Id, userRole);
            return await _userManager.AddToRolesAsync(user, userRole);
        }
    }
}
