using System.Threading.Tasks;
using Core.Enums;
using Core.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Web.Hubs;

namespace Web.Controllers {
    [Authorize]
    public class AdminController: BaseController<AdminController> {
        private readonly ISyncBusinessService _syncBusinessService;

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

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController: ControllerBase {
        private readonly ISyncBusinessService _syncBusinessService;
        private readonly IHubContext<SyncDataHub> _syncDataHubContext;

        public AdminController(IHubContext<SyncDataHub> syncDataHubContext, ISyncBusinessService syncBusinessService) {
            _syncDataHubContext = syncDataHubContext;
            _syncBusinessService = syncBusinessService;
        }

        [HttpGet]
        [Route("startSync")]
        public async Task<IActionResult> StartSync(NsiEnum nsitype) {
            var result = await _syncBusinessService.Sync(nsitype);
            if(result != null) {
               await _syncDataHubContext.Clients.All.SendAsync("syncNsiResult", result);
            }
            return Ok(result);
        }

    }
}