using System;

using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Web.Models;
using Web.Models.DocumentViewModel;

namespace Web.Controllers {
    [Authorize]
    public class HomeController: BaseController<HomeController> {
        public HomeController(IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<HomeController> localizer,
            ILogger<HomeController> logger) : base(httpContextAccessor, localizer, logger) {

        }

        public IActionResult Index() {
            var searchCriteria = new SearchViewModel();


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
