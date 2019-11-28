using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Core.Data.Dto;
using Core.Data.Entities;

namespace Core.Config {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<ApplicationUserEntity, ApplicationUserDto>().ReverseMap();
            CreateMap<UserProfileEntity, UserProfileDto>().ReverseMap();
        }
    }
}
