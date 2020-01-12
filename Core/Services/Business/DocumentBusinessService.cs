using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto.Documents;
using Core.Data.Entities.Documents;
using Core.Extensions;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IDocumentBusinessService {
        Task<PagerExtended<DocumentDto>> GetListOfDocument(SearchDto search, string sort, string order, int offset, int limit);
        Task<DocumentDto> GetDocument(Guid id);
        Task<DocumentDto> GetDocumentByNgr(string ngr, int lng, DateTime editionDate);
        Task<List<DocumentDto>> GetDocumentsByNgr(string ngr);
    }

    public class DocumentBusinessService: IDocumentBusinessService {
        private readonly IMapper _mapper;
        private readonly IDocumentManager _documentManager;
        private readonly IDocumentBodyManager _documentBodyManager;

        public DocumentBusinessService(IMapper mapper,
            IDocumentManager documentManager,
            IDocumentBodyManager documentBodyManager) {
            _mapper = mapper;
            _documentManager = documentManager;
            _documentBodyManager = documentBodyManager;
        }

        public async Task<PagerExtended<DocumentDto>> GetListOfDocument(SearchDto search, string sort, string order, int offset, int limit) {
            Expression<Func<DocumentEntity, bool>> where = x =>
                (true)
                && ((string.IsNullOrEmpty(search.SearchText)) || (x.Title.Contains(search.SearchText.Trim()) || x.Info.Contains(search.SearchText.Trim())))
                && ((search.Languages == null || search.Languages.Count == 0) || search.Languages.Contains(x.Language.Id))
                && ((search.Statuses == null || search.Statuses.Count == 0) || search.Statuses.Contains(x.Status.Id))
                // && (search.AcceptedRegions.Contains(x.AcceptedRegion.Id) || false)
                //&& ((search.DocumentTypes == null || search.DocumentTypes.Count == 0) || search.DocumentTypes.Contains(x.NsiDocumentSectionEntity_Id.Id) || false)
                && ((search.DocumentSections == null || search.DocumentSections.Count == 0) || search.DocumentSections.Contains(x.Section.Id) || false)

                && (x.IsArchive == false);

            var sortby = GetExpression<DocumentEntity>(null ?? "Name");

            Tuple<List<DocumentEntity>, int> tuple = await _documentManager.Pager<DocumentEntity>(where, sortby, !order.Equals("asc"), offset, limit, new string[] { "Status", "Section" });
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerExtended<DocumentDto>(new List<DocumentDto>(), 0, offset, limit);

            var page = (offset + limit) / limit;

            var result = _mapper.Map<List<DocumentDto>>(list);
            return new PagerExtended<DocumentDto>(result, count, page, limit);
        }

        public async Task<DocumentDto> GetDocument(Guid id) {
            var item = await _documentManager.FindInclude(id);
            if(item != null) {
                var itemBody = await _documentBodyManager.FindByDocumentIdAsync(id);
                var dto = _mapper.Map<DocumentDto>(item);
                dto.Content = _mapper.Map<DocumentBodyDto>(itemBody);

                return dto;
            }
            return null;
        }

        public async Task<DocumentDto> GetDocumentByNgr(string ngr, int lng, DateTime editionDate) {
            var item = await _documentManager.FindByNgrAsync(ngr, lng, editionDate);
            if(item != null) {
                var itemBody = await _documentBodyManager.FindByDocumentIdAsync(item.Id);
                var dto = _mapper.Map<DocumentDto>(item);
                dto.Content = _mapper.Map<DocumentBodyDto>(itemBody);

                return dto;
            }
            return null;
        }

        public async Task<List<DocumentDto>> GetDocumentsByNgr(string ngr) {
            var item = await _documentManager.FindAllByNgrAsync(ngr);
            return _mapper.Map<List<DocumentDto>>(item);
        }

        public static Expression<Func<TSource, string>> GetExpression<TSource>(string propertyName) {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(string));
            return Expression.Lambda<Func<TSource, string>>(conversion, param);
        }
    }
}
