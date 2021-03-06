﻿using AutoMapper;
using Core.Data.Dto.DbStatus;
using Core.Data.Dto.Documents;
using Core.Data.Dto.Nsi;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Web.Models.Document;
using Web.Models.ViewModels.Document;
using Web.ViewModels.DbStatus;

namespace Web.App_Config {
    public class MapperConfig: Profile {
        public static void Register(IServiceCollection services) {
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new Core.Config.MapperConfig());
                mc.AddProfile(new MapperConfig());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddMvc();
        }

        private MapperConfig() {
            CreateMap<DocumentViewModel, DocumentDto>()
                .ReverseMap()
                .ForMember(d => d.Content, o => o.MapFrom(s => (s.Content != null) ? s.Content.Content : ""))
                ;

            CreateMap<SearchViewModel, SearchDto>()
                .ForMember(d => d.SearchText, o => o.MapFrom(s => s.Search.Value))
                .ForMember(d => d.Regex, o => o.MapFrom(s => s.Search.Regex))
                .ReverseMap();

            CreateMap<NsiDto<string>, SelectListItem>()
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Text, o => o.MapFrom(s => s.NameRu));

            CreateMap<DbStatusDocumentViewModel, DbStatusDocumentDto>().ReverseMap();
        }
    }
}
