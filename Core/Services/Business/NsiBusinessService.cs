using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Core.Data.Dto.Nsi;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface INsiBusinessService {
        Task<List<NsiDto<int>>> GetLanguages();
        Task<List<NsiDto<int>>> GetDocumentStatues();
        Task<List<NsiDto<Guid>>> GetDocumentTypes();
        Task<List<NsiDto<Guid>>> GetRegions(string parentId);
        Task<List<NsiDto<Guid>>> GetDocumentSections();
        
        Task<List<NsiDto<Guid>>> GetDocumentTitlePrefixes();

    }
    public class NsiBusinessService: INsiBusinessService {
        private readonly IMapper _mapper;

        private readonly INsiLanguageManager _nsiLanguagesManager;
        private readonly INsiDocumentStatusManager _nsiDocumentStatusesManager;
        private readonly INsiDocumentTypeManager _nsiDocumentTypeManager;
        private readonly INsiRegionManager _nsiRegionManager;
        //private readonly INsiDevAgencyManager _nsiDevAgencyManager;
        //private readonly INsiInitRegionManager _nsiInitRegionManager;
        private readonly INsiDocumentSectionManager _nsiDocumentSectionManager;
        //private readonly INsiSourceManager _nsiSourceManager;
        //private readonly INsiRegAgencyManager _nsiRegAgencyManager;
        //private readonly INsiClassifierManager _nsiClassifierManager;
        //private readonly INsiDepartmentManager _nsiDepartmentManager;
        private readonly INsiDocumentTitlePrefixManager _nsiDocumentTitlePrefixManager;
        //private readonly INsiLawForceManager _nsiLawForceManager;
        //private readonly INsiGrifTypeManager _nsiGrifTypeManager;

        public NsiBusinessService(IMapper mapper, INsiLanguageManager nsiLanguagesManager,
            INsiDocumentStatusManager nsiDocumentStatusManager,
            INsiDocumentTypeManager nsiDocumentTypeManager,
            INsiRegionManager nsiRegionManager,
            INsiDocumentSectionManager nsiDocumentSectionManager,
            INsiDocumentTitlePrefixManager nsiDocumentTitlePrefixManager) {
            _mapper = mapper;
            _nsiLanguagesManager = nsiLanguagesManager;
            _nsiDocumentStatusesManager = nsiDocumentStatusManager;
            _nsiDocumentTypeManager = nsiDocumentTypeManager;
            _nsiRegionManager = nsiRegionManager;
            _nsiDocumentSectionManager = nsiDocumentSectionManager;
            _nsiDocumentTitlePrefixManager = nsiDocumentTitlePrefixManager;
        }

        public async Task<List<NsiDto<int>>> GetLanguages() {
            var result = await _nsiLanguagesManager.All();
            return _mapper.Map<List<NsiDto<int>>>(result);
        }

        public async Task<List<NsiDto<int>>> GetDocumentStatues() {
            var result = await _nsiDocumentStatusesManager.FindByCodesAsync(new string[] { "НЕТ", "YTS", "STP" });
            return _mapper.Map<List<NsiDto<int>>>(result);
        }

        public async Task<List<NsiDto<Guid>>> GetDocumentTypes() {
            var result = await _nsiDocumentTypeManager.All();
            return _mapper.Map<List<NsiDto<Guid>>>(result);
        }

        public async Task<List<NsiDto<Guid>>> GetDocumentSections() {
            var result = await _nsiDocumentSectionManager.All();
            return _mapper.Map<List<NsiDto<Guid>>>(result);
        }

        public async Task<List<NsiDto<Guid>>> GetRegions(string parentId) {
            var newGuid = Guid.Empty;
            Guid.TryParse(parentId, out newGuid);

            var result = await _nsiRegionManager.FindByParentId(newGuid);
            
            return _mapper.Map<List<NsiDto<Guid>>>(result);
        }

        public async Task<List<NsiDto<Guid>>> GetDocumentTitlePrefixes() {
            var result = await _nsiDocumentTitlePrefixManager.All();
            return _mapper.Map<List<NsiDto<Guid>>>(result);
        }
    }
}
