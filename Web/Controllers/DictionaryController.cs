using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Web.Controllers {
    public class DictionaryController: BaseController<DictionaryController> {
        private readonly IMapper _mapper;
        public DictionaryController(IMapper mapper,
                IHttpContextAccessor httpContextAccessor,
                 IStringLocalizer<DictionaryController> localizer,
                 ILogger<DictionaryController> logger) : base(httpContextAccessor, localizer, logger) {
            _mapper = mapper;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Terms() {
            return View();
        }

        public IActionResult LawTerms() {
            return View();
        }
    }
}