using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Core.Common {
    public class LanguageRouteConstraint: IRouteConstraint {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection) {
            if(!values.ContainsKey("culure"))
                return false;

            var lang  = values["culture"].ToString();
            if(!string.IsNullOrWhiteSpace(lang) && (lang.Equals("ru") || lang.Equals("kk") || lang.Equals("en"))) {
                var culture = new CultureInfo(lang);
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            return lang == "kk" || lang == "ru" || lang == "en";
        }
    }
}
