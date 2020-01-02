using System;
using System.Diagnostics;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto.Documents;
using Core.Services.Business;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.Models;
using Web.Models.DocumentViewModel;

namespace Web.Controllers {
    [Authorize]
    public class HomeController: BaseController<HomeController> {
        private readonly IMapper _mapper;
        private readonly IDocumentBusinessService _documentBusinessService;
        private readonly INsiBusinessService _nsiBusinessService;

        public HomeController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<HomeController> localizer,
            ILogger<HomeController> logger,
            IDocumentBusinessService documentBusinessService,
            INsiBusinessService nsiBusinessService) : base(httpContextAccessor, localizer, logger) {
            _mapper = mapper;
            _documentBusinessService = documentBusinessService;
            _nsiBusinessService = nsiBusinessService;
        }

        public async Task<IActionResult> Index() {
            var searchCriteria = new SearchViewModel();
            // var result = await _documentBusinessService.GetListOfDocument(new SearchDto(), "", "", 0, 100);

            ViewBag.Languages = await _nsiBusinessService.GetLanguages();
            ViewBag.Statuses = await _nsiBusinessService.GetDocumentStatues();
            //ViewBag.DocumentTypes = await _nsiBusinessService.GetDocumentTypes();
            ViewBag.DocumentSections = await _nsiBusinessService.GetDocumentSections();
            ViewBag.Regions = await _nsiBusinessService.GetRegions(null);
            ViewBag.DocumentTitlePrefixes = await _nsiBusinessService.GetDocumentTitlePrefixes();
            //result.Languages = _mapper.Map<List<SelectListItem>>(lang);

            //var result = _documentBusinessService.GetListOfDocument(_mapper.Map<SearchDto>(searchCriteria));

            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        public IActionResult SetCulture(string id = "en") {
            var culture = id;
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl) {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        //public IActionResult SetRegion(string regionCode) {
        //    if(string.IsNullOrEmpty(regionCode)) {
        //        Response.Cookies.Delete("currentRegionEn");
        //        Response.Cookies.Delete("currentRegionRu");
        //        Response.Cookies.Delete("currentRegionKk");
        //    } else {
        //        //var region = _nsi.GetRegionByCodeMore(regionCode);

        //        CookieOptions options = new CookieOptions();
        //        options.Expires = DateTime.Now.AddDays(1);
        //        //Response.Cookies.Append("currentRegionEn", $"{region.Code}:{region.Name}");
        //        //Response.Cookies.Append("currentRegionRu", $"{region.Code}:{region.NameRu}");
        //        //Response.Cookies.Append("currentRegionKk", $"{region.Code}:{region.NameKz}");
        //    }
        //    return Redirect($"/{CurrentLanguage}");
        //}

        //public IActionResult SetLanguage(string culture, string returnUrl) {
        //    Response.Cookies.Append(
        //        CookieRequestCultureProvider.DefaultCookieName,
        //        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
        //        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        //    );
        //    if(string.IsNullOrEmpty(returnUrl)) {
        //        returnUrl = "/" + culture;
        //    } else {
        //        returnUrl = returnUrl.Replace(";", "&");
        //        returnUrl = "/" + culture; //+ returnUrl.Substring(3, returnUrl.Length - 3);
        //    }
        //    return LocalRedirect(returnUrl);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ISyncBusinessService _syncBusinessService;
        private readonly IHubContext<SyncDataHub> _syncDataHubContext;
        private readonly IDocumentBusinessService _documentBusinessService;

        public HomeController(IMapper mapper, IHubContext<SyncDataHub> syncDataHubContext, ISyncBusinessService syncBusinessService,
            IDocumentBusinessService documentBusinessService) {
            _mapper = mapper;
            _syncDataHubContext = syncDataHubContext;
            _syncBusinessService = syncBusinessService;
            _documentBusinessService = documentBusinessService;
        }

        [HttpPost]
        public async Task<IActionResult> GetPager([FromForm] SearchViewModel model) {
            var search = _mapper.Map<SearchDto>(model);
            var item = await _documentBusinessService.GetListOfDocument(search, "Id", "asc", search.Start, search.Length);
            return Ok(item);
        }
    }
}