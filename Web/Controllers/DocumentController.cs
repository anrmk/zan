using System;
using System.Text;
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

using System.IO;
using SelectPdf;

namespace Web.Controllers {
    public class DocumentController: BaseController<DocumentController> {
        private readonly IMapper _mapper;
        private readonly IExportService _exportService;
        private readonly IViewRenderService _viewRenderService;
        private readonly IDocumentBusinessService _documentBusinessService;
        private readonly INsiBusinessService _nsiBusinessService;

        public DocumentController(IMapper mapper,
          IViewRenderService viewRenderService,
          IExportService exportService,
          IHttpContextAccessor httpContextAccessor,
          IStringLocalizer<DocumentController> localizer,
          ILogger<DocumentController> logger,
          IDocumentBusinessService documentBusinessService,
          INsiBusinessService nsiBusinessService) : base(httpContextAccessor, localizer, logger) {
            _mapper = mapper;
            _exportService = exportService;
            _viewRenderService = viewRenderService;
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

        /// <summary>
        /// Печать документа HTML
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Print(Guid id) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<DocumentViewModel>(item);

            ViewBag.Title = string.Format("{0}_{1}.doc", model.Ngr, model.EditionDate?.ToString("ddMMyyyy")); ;
            ViewBag.OnLoad = "window.print()";

            return View("_ExportToWord", model);
        }

        [HttpPost]
        public async Task<ActionResult> PrintFragment(Guid id, string selected) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<DocumentViewModel>(item);
            model.Content = $"<div>{selected}</div>";

            ViewBag.Title = string.Format("{0}_{1}.doc", model.Ngr, model.EditionDate?.ToString("ddMMyyyy")); ;
            ViewBag.OnLoad = "window.print()";

            return View("_ExportToWord", model);
        }

        /// <summary>
        /// Выгрузка в PDF
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> PrintPdf(Guid id) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<DocumentViewModel>(item);
            string html = _viewRenderService.RenderToStringAsync("_ExportToWord", model).Result;
            var name = string.Format("{0}_{1}.pdf", item.Ngr, (item.EditionDate != null) ? item.EditionDate.ToString("ddMMyyyy") : "");

            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;

            PdfDocument doc = converter.ConvertHtmlString(html);

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();
            
            stream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = name;

            return fileStreamResult;
        }

        [HttpPost]
        public async Task<ActionResult> PrintFragmentPdf(Guid id, string selected) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<DocumentViewModel>(item);
            model.Content = $"<div>{selected}</div>";
            string html = _viewRenderService.RenderToStringAsync("_ExportToWord", model).Result;
            var name = string.Format("{0}_{1}_Fragment.pdf", item.Ngr, (item.EditionDate != null) ? item.EditionDate.ToString("ddMMyyyy") : "");

            HtmlToPdf converter = new HtmlToPdf();

            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;

            PdfDocument doc = converter.ConvertHtmlString(html);

            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            doc.Close();

            stream.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");
            fileStreamResult.FileDownloadName = name;

            return fileStreamResult;
        }

        /// <summary>
        /// Выгрузка в DOC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> PrintWord(Guid id) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<DocumentViewModel>(item);
            var name = string.Format("{0}_{1}.doc", model.Ngr, model.EditionDate?.ToString("ddMMyyyy"));

            Response.Headers.Add("content-disposition", $"attachment; filename = {name}");
            Response.ContentType = "application/ms-word";
            return View("_ExportToWord", model);
        }

        [HttpPost]
        public async Task<ActionResult> PrintFragmentWord(Guid id, string selected) {
            var item = await _documentBusinessService.GetDocument(id);
            if(item == null) {
                return NotFound();
            }

            var model = _mapper.Map<DocumentViewModel>(item);
            model.Content = $"<div>{selected}</div>";
            var name = string.Format("{0}_{1}.doc", model.Ngr, model.EditionDate?.ToString("ddMMyyyy"));

            Response.Headers.Add("content-disposition", $"attachment; filename = {name}");
            Response.ContentType = "application/ms-word";
            return View("_ExportToWord", model);
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