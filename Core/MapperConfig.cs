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
                 .ForMember(d => d.Status, o => o.MapFrom(s => s.Status != null ? s.Status.NameRu : ""))
                 .ForMember(d => d.Section, o => o.MapFrom(s => s.Section != null ? s.Section.NameRu : ""))
                 .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType != null ? s.DocumentType.NameRu : ""))
                 .ForMember(d => d.Language, o => o.MapFrom(s => s.Language != null ? s.Language.NameRu : ""))
                 .ForMember(d => d.DevAgency, o => o.MapFrom(s => s.DevAgency != null ? s.DevAgency.NameRu : ""))
                 .ForMember(d => d.RegAgency, o => o.MapFrom(s => s.RegAgency != null ? s.RegAgency.NameRu : ""))
                 .ForMember(d => d.RegionAction, o => o.MapFrom(s => s.RegionAction != null ? s.RegionAction.NameRu : ""))
                 .ForMember(d => d.AcceptedRegion, o => o.MapFrom(s => s.AcceptedRegion != null ? s.AcceptedRegion.NameRu : ""))
                 .ForMember(d => d.InitRegion, o => o.MapFrom(s => s.InitRegion != null ? s.InitRegion.NameRu : ""))
                 .ForMember(d => d.Classifier, o => o.MapFrom(s => s.Classifier != null ? s.Classifier.NameRu : ""))
                 .ForMember(d => d.LawForce, o => o.MapFrom(s => s.LawForce != null ? s.LawForce.NameRu : ""))
                .ReverseMap();

            CreateMap<DocumentBodyEntity, DocumentBodyDto>().ReverseMap();

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
