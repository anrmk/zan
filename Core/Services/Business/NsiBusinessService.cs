using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.Data.Dto.Nsi;
using Core.Services.Managers;

namespace Core.Services.Business {
    public interface INsiBusinessService {
        Task<NsiDto> GetLanguages();
    }
    public class NsiBusinessService: INsiBusinessService {
        private readonly IMapper _mapper;
        private readonly INsiLanguageManager _nsiLanguagesManager;

        public NsiBusinessService(IMapper mapper, INsiLanguageManager nsiLanguagesManager) {
            _mapper = mapper;
            _nsiLanguagesManager = nsiLanguagesManager;
        }

        public async Task<NsiDto> GetLanguages() {
            var result = await _nsiLanguagesManager.All();
            return _mapper.Map<NsiDto>(result);
        }
    }
}
