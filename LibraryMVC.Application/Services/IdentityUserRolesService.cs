using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.IdentityUserRoles;
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

        public IdentityUserRolesService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
    }
}
