﻿using FluentValidation;
using FluentValidation.AspNetCore;
using LibraryMVC.Application.Interfaces;
using LibraryMVC.Application.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryMVC.Web.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class LibraryUserController : Controller
    {
        private readonly ILibraryUserService _userService;
        private readonly IValidator<NewLibraryUserVm> _validator;
        public LibraryUserController(ILibraryUserService userService, IValidator<NewLibraryUserVm> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var model = _userService.GetAllLibraryUsersForList();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddNewUser() 
        {
            return View(new NewLibraryUserVm());
        }

        [HttpPost]
        public IActionResult AddNewUser(NewLibraryUserVm model)
        {
            var result = _validator.Validate(model);
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(model);
            }

            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var identityUserEmail = User.FindFirstValue(ClaimTypes.Email);
            model.IdentityUserId = identityUserId;
            model.Email = identityUserEmail;
            _userService.AddUser(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
