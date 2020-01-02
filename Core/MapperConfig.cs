using System;
using AutoMapper;

using Core.Data.Dto;
using Core.Data.Dto.Documents;
using Core.Data.Dto.Nsi;
using Core.Data.Entities;
using Core.Data.Entities.Documents;
using Core.Data.Entities.Nsi;

namespace Core.Config {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<ApplicationUserEntity, ApplicationUserDto>().ReverseMap();
            CreateMap<UserProfileEntity, UserProfileDto>().ReverseMap();
            CreateMap<DocumentEntity, DocumentDto>()
                 .ForMember(d => d.StatusId, o => o.MapFrom(s => s.NsiDocumentStatusEntity_Id))
                 .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status != null ? s.Status.NameRu : ""))
                 .ForMember(d => d.SectionId, o => o.MapFrom(s => s.NsiDocumentSectionEntity_Id))
                 .ForMember(d => d.SectionName, o => o.MapFrom(s => s.Section != null ? s.Section.NameRu : ""))

                .ReverseMap();

            CreateMap<NsiLanguageEntity, NsiDto<int>>().ReverseMap(); //CodeImport
            CreateMap<NsiDocumentTypeEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiRegionEntity, NsiDto<Guid>>().ReverseMap(); //CodeRu, CodeKk, CodeEn
            CreateMap<NsiDocumentStatusEntity, NsiDto<int>>().ReverseMap(); //CodeBd7
            CreateMap<NsiDevAgencyEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiInitRegionEntity, NsiDto<Guid>>().ReverseMap(); //OldId
            CreateMap<NsiDocumentSectionEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiSourceEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiRegAgencyEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiClassifierEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiDepartmentEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiDocumentTitlePrefixEntity, NsiDto<Guid>>().ReverseMap(); //CodeRu, CodeKk, CodeEn
            CreateMap<NsiLawForceEntity, NsiDto<Guid>>().ReverseMap();
            CreateMap<NsiGrifTypeEntity, NsiDto<Guid>>().ReverseMap();
        }
    }
}
