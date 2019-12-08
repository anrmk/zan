using Core.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    [Authorize]
    public class AdminController: BaseController<AdminController> {
        private ISyncBusinessService _syncBusinessService;

        public AdminController(IHttpContextAccessor httpContextAccessor,
           IStringLocalizer<AdminController> localizer,
           ILogger<AdminController> logger,
           ISyncBusinessService syncBusinessService) : base(httpContextAccessor, localizer, logger) {

            _syncBusinessService = syncBusinessService;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Sync() {
            return View();
        }
    }
}