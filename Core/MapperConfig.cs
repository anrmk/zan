using AutoMapper;
using Core.Data.Dto;
using Core.Data.Dto.Nsi;
using Core.Data.Entities;
using Core.Data.Entities.Nsi;

namespace Core.Config {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<ApplicationUserEntity, ApplicationUserDto>().ReverseMap();
            CreateMap<UserProfileEntity, UserProfileDto>().ReverseMap();

            CreateMap<NsiLanguageEntity, NsiDto>().ReverseMap(); //CodeImport
            CreateMap<NsiDocumentTypeEntity, NsiDto>().ReverseMap();
            CreateMap<NsiRegionEntity, NsiDto>().ReverseMap(); //CodeRu, CodeKk, CodeEn
            CreateMap<NsiDocumentStatusEntity, NsiDto>().ReverseMap(); //CodeBd7
            CreateMap<NsiDevAgencyEntity, NsiDto>().ReverseMap();
            CreateMap<NsiInitRegionEntity, NsiDto>().ReverseMap(); //OldId
            CreateMap<NsiDocSectionEntity, NsiDto>().ReverseMap();
            CreateMap<NsiSourceEntity, NsiDto>().ReverseMap();
            CreateMap<NsiRegAgencyEntity, NsiDto>().ReverseMap();
            CreateMap<NsiClassifierEntity, NsiDto>().ReverseMap();
            CreateMap<NsiDepartmentEntity, NsiDto>().ReverseMap();
            CreateMap<NsiDocTitlePrefixEntity, NsiDto>().ReverseMap(); //CodeRu, CodeKk, CodeEn
        }
    }
}
