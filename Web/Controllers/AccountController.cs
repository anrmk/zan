using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Dto;
using Core.Services.Business;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers {
   // [Route("[controller]/[action]")]
    public class AccountController: Controller {
        private readonly IAccountBusinessService _accountBusinessService;

        public AccountController(IAccountBusinessService accountBusinessService) {
            _accountBusinessService = accountBusinessService;
        }

        public async Task<IActionResult> Index() {
            /*var user = _accountBusinessService.CreateUser(new ApplicationUserDto() {
                UserName = "Admin",
                NormalizedUserName = "Administrator",
                Email = "test@test.com",
                EmailConfirmed = true,
                PhoneNumber = "3239226969",
                PhoneNumberConfirmed = true
            });*/

            var xxx = await _accountBusinessService.UpdateUserProfile("1111", new UserProfileDto() { });

            return View();
        }
    }
}