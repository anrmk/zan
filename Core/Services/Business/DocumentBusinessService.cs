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
    }

    public class DocumentBusinessService: IDocumentBusinessService {
        private readonly IMapper _mapper;
        private readonly IDocumentManager _documentManager;

        public DocumentBusinessService(IMapper mapper, IDocumentManager documentManager) {
            _mapper = mapper;
            _documentManager = documentManager;
        }

        public async Task<PagerExtended<DocumentDto>> GetListOfDocument(SearchDto search, string sort, string order, int offset, int limit) {
            Expression<Func<DocumentEntity, bool>> where = x =>
                (true)
                && ((string.IsNullOrEmpty(search.SearchText)) || x.Title.Contains(search.SearchText.Trim()))
                && ((search.Languages == null || search.Languages.Count == 0) || search.Languages.Contains(x.Language.Id))
                && ((search.Statuses == null  || search.Statuses.Count == 0) || search.Statuses.Contains(x.Status.Id))
                // && (search.AcceptedRegions.Contains(x.AcceptedRegion.Id) || false)
                //&& ((search.DocumentTypes == null || search.DocumentTypes.Count == 0) || search.DocumentTypes.Contains(x.NsiDocumentSectionEntity_Id.Id) || false)
                && ((search.DocumentSections == null || search.DocumentSections.Count == 0) || search.DocumentSections.Contains(x.Section.Id) || false)

                && (x.IsArchive == false);

            var sortby = GetExpression<DocumentEntity>(null ?? "Name");

            Tuple<List<DocumentEntity>, int> tuple = await _documentManager.Pager<DocumentEntity>(where, sortby, !order.Equals("asc"), offset, limit);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new PagerExtended<DocumentDto>(new List<DocumentDto>(), 0, offset, limit);

            var page = (offset + limit) / limit;

            var result = _mapper.Map<List<DocumentDto>>(list);
            return new PagerExtended<DocumentDto>(result, count, page, limit);
        }

        public static Expression<Func<TSource, string>> GetExpression<TSource>(string propertyName) {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(string));
            return Expression.Lambda<Func<TSource, string>>(conversion, param);
        }

    }
}
