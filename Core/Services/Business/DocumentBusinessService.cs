using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto.Documents;
using Core.Data.Entities.Documents;
using Core.Extensions;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface IDocumentBusinessService {
        Task<PagerExtended<DocumentDto>> GetListOfDocument(Guid userId, SearchDto search, int offset, int limit);
        Task<DocumentDto> GetDocument(Guid id);
        Task<DocumentDto> GetDocumentByNgr(string ngr, int lng, DateTime editionDate);
        Task<List<DocumentDto>> GetDocumentsByNgr(string ngr);

        Task<List<DocumentDto>> GetFavorites(Guid userId, int limit);
        Task<DocumentDto> AddToFavorite(Guid userId, Guid documentId);
    }

    public class DocumentBusinessService: IDocumentBusinessService {
        private readonly IMapper _mapper;
        private readonly IDocumentManager _documentManager;
        private readonly IDocumentBodyManager _documentBodyManager;
        private readonly IDocumentFavoriteManager _documentFavoriteManager;

        public DocumentBusinessService(IMapper mapper,
            IDocumentManager documentManager,
            IDocumentBodyManager documentBodyManager,
            IDocumentFavoriteManager documentFavoriteManager) {
            _mapper = mapper;
            _documentManager = documentManager;
            _documentBodyManager = documentBodyManager;
            _documentFavoriteManager = documentFavoriteManager;
        }

        public async Task<PagerExtended<DocumentDto>> GetListOfDocument(Guid userId, SearchDto search, int offset, int limit) {
            #region FAVORITE
            var favoriteDocumentIds = new List<Guid?>();

            var favoriteEntity = await _documentFavoriteManager.FindListByUserIdAsync(userId);
            if(favoriteEntity != null)
                favoriteDocumentIds = favoriteEntity.Select(x => x.DocumentId).ToList();

            #endregion

            Expression<Func<DocumentEntity, bool>> where = x =>
                (true)
                && ((favoriteDocumentIds.Count > 0) ? favoriteDocumentIds.Contains(x.Id) : true)
                && ((string.IsNullOrEmpty(search.SearchText)) || (x.Title.Contains(search.SearchText.Trim()) || x.Info.Contains(search.SearchText.Trim())))
                && ((search.Languages == null || search.Languages.Count == 0) || search.Languages.Contains(x.Language.Id))
                && ((search.Statuses == null || search.Statuses.Count == 0) || search.Statuses.Contains(x.Status.Id))
                // && (search.AcceptedRegions.Contains(x.AcceptedRegion.Id) || false)
                //&& ((search.DocumentTypes == null || search.DocumentTypes.Count == 0) || search.DocumentTypes.Contains(x.NsiDocumentSectionEntity_Id.Id) || false)
                && ((search.DocumentSections == null || search.DocumentSections.Count == 0) || search.DocumentSections.Contains(x.Section.Id) || false)

                && (x.IsArchive == false);

            #region SORT EXPRESSION
            string sortby;
            switch(search.Sort) {
                case 1: sortby = "Id"; break;
                case 2: sortby = "LawForceId"; break;
                case 3: sortby = "AcceptedDate"; break;
                case 4: sortby = "EditionDate"; break;
                default: sortby = "Id"; break;
            }
            #endregion

            try {

                Tuple<List<DocumentEntity>, int> tuple = await _documentManager.Pager<DocumentEntity>(where, sortby, search.SortByDesc, offset, limit, new string[] { "Status", "Section" });
                var list = tuple.Item1;
                var count = tuple.Item2;

                if(count == 0)
                    return new PagerExtended<DocumentDto>(new List<DocumentDto>(), 0, offset, limit);

                var page = (offset + limit) / limit;

                var result = _mapper.Map<List<DocumentDto>>(list);
                return new PagerExtended<DocumentDto>(result, count, page, limit);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            return new PagerExtended<DocumentDto>(new List<DocumentDto>(), 0, offset, limit);
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

        /// <summary>
        /// Получение списка избранных документов
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<DocumentDto>> GetFavorites(Guid userId, int limit = 10) {
            var favorites = await _documentFavoriteManager.FindListByUserIdAsync(userId);
            var favoritesIds = favorites.Take(limit).Select(x => x.DocumentId).ToArray();

            var item = await _documentManager.FindAllByIds(favoritesIds);
            return _mapper.Map<List<DocumentDto>>(item);
        }

        public async Task<DocumentDto> AddToFavorite(Guid userId, Guid documentId) {
            var item = await _documentFavoriteManager.FindByUserIdAsync(userId, documentId);
            if(item == null) {
                item = await _documentFavoriteManager.CreateOrUpdate(new DocumentFavoriteEntity() {
                    ApplicationUserId = userId,
                    DocumentId = documentId
                });
            }

            var result = await _documentManager.FindInclude(documentId);
            return _mapper.Map<DocumentDto>(result);
        }
    }
}
