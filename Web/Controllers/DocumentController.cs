using System;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto.Documents;
using Core.Extensions;
using Core.Services.Business;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Web.Hubs;
using Web.Models.Document;
using Web.Models.ViewModels.Document;

namespace Web.Controllers {
    public class DocumentController: BaseController<DocumentController> {
        private readonly IMapper _mapper;
        private readonly IDocumentBusinessService _documentBusinessService;
        private readonly INsiBusinessService _nsiBusinessService;

        public DocumentController(IMapper mapper, IHttpContextAccessor httpContextAccessor,
          IStringLocalizer<DocumentController> localizer,
          ILogger<DocumentController> logger,
          IDocumentBusinessService documentBusinessService,
          INsiBusinessService nsiBusinessService) : base(httpContextAccessor, localizer, logger) {
            _mapper = mapper;
            _documentBusinessService = documentBusinessService;
            _nsiBusinessService = nsiBusinessService;
        }

        // GET: Document
        public async Task<ActionResult> Index() {
            ViewBag.Languages = await _nsiBusinessService.GetLanguages();
            ViewBag.Statuses = await _nsiBusinessService.GetDocumentStatues();
            //ViewBag.DocumentTypes = await _nsiBusinessService.GetDocumentTypes();
            ViewBag.DocumentSections = await _nsiBusinessService.GetDocumentSections();
            ViewBag.Regions = await _nsiBusinessService.GetRegions(null);
            ViewBag.DocumentTitlePrefixes = await _nsiBusinessService.GetDocumentTitlePrefixes();

            return View();
        }

        // GET: Document/Details/5
        public async Task<ActionResult> Details(Guid id) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }
            item.Versions = await _documentBusinessService.GetDocumentsByNgr(item.Ngr);
            var model = _mapper.Map<DocumentViewModel>(item);
            ViewBag.Languages = DocumentDtoExtension.GetAvailableLanguages(item.Versions, item.Ngr, item.EditionDate);
            ViewBag.Versions = DocumentDtoExtension.GetAvailableVersions(item.Versions, item.Ngr, item.LanguageId);

            return View(model);
        }

        // GET: Document/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Document/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: Document/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        // POST: Document/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            try {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: Document/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        // POST: Document/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            try {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }
    }
}

namespace Web.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController: ControllerBase {
        private readonly IMapper _mapper;
        private readonly ISyncBusinessService _syncBusinessService;
        private readonly IHubContext<SyncDataHub> _syncDataHubContext;
        private readonly IDocumentBusinessService _documentBusinessService;

        public DocumentController(IMapper mapper, IHubContext<SyncDataHub> syncDataHubContext, ISyncBusinessService syncBusinessService,
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