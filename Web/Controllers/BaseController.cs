using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    public class BaseController<IController>: Controller {
        public string CurrentLanguage => "ru";
        protected IController controller;
        //protected TRepository _repository { get; private set; }

        private readonly IStringLocalizer<IController> _localizer;
        private readonly ILogger<IController> _logger;

        public BaseController(IHttpContextAccessor httpContextAccessor, IStringLocalizer<IController> localizer, ILogger<IController> logger) {
            //_repository = repository;
            //_repository.User = httpContextAccessor.HttpContext.User;
            //_repository.Language = CurrentLanguage;
            _localizer = localizer;
            _logger = logger;
        }

        public override ViewResult View(string view, object model) {
            this.ViewBag.Language = this.CurrentLanguage;
            return base.View(view, model);
        }

        public override ViewResult View(object model) {
            this.ViewBag.Language = this.CurrentLanguage;
            return base.View(model);
        }

        public override ViewResult View() {
            this.ViewBag.Language = this.CurrentLanguage;
            return base.View();
        }
    }
}