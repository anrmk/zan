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
    public interface IDocumentBusinessService { }

    public class DocumentBusinessService: IDocumentBusinessService {
        private readonly IMapper _mapper;
        private readonly IDocumentManager _documentManager;

        public DocumentBusinessService(IMapper mapper, DocumentManager documentManager) {
            _mapper = mapper;
            _documentManager = documentManager;
        }

        public async Task<Pager<DocumentDto>> GetListOfDocument(SearchDto search) {
            //(string search, string sort = "Id", string order = "asc", int limit = 20, int offset = 0)
            // var result = _documentManager.Search(search);
            Expression<Func<DocumentEntity, bool>> where = x =>
                (true)
                // && (search.Languages.Contains(x.Language.Id))
                && (x.Status.Name == search.SelectedStatuses)
                && (x.IsArchive == false);

            var sortby = GetExpression<DocumentEntity>(null ?? "Name");

            Tuple<List<DocumentEntity>, int> tuple = await _documentManager.Pager<DocumentEntity>(where, sortby, search.SordByDesc, search.Total * search.TotalPages, search.Limit);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<DocumentDto>(new List<DocumentDto>(), 0, search.Page, search.Limit);

            var page = (search.Total + search.Limit) / search.Limit;

            var result = _mapper.Map<List<DocumentDto>>(list);
            return new Pager<DocumentDto>(result, count, page, search.Limit);
        }

        public static Expression<Func<TSource, string>> GetExpression<TSource>(string propertyName) {
            var param = Expression.Parameter(typeof(TSource), "x");
            Expression conversion = Expression.Convert(Expression.Property(param, propertyName), typeof(string));
            return Expression.Lambda<Func<TSource, string>>(conversion, param);
        }

    }
}
