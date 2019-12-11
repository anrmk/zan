﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Core.Data.Dto;
using Core.Data.Entities;
using Core.Services.Managers;
using Microsoft.AspNetCore.Identity;

namespace Core.Services.Business {
    public interface IAccountBusinessService {
        //Task<Pager<ApplicationUserDtoList>> GetPager(Dictionary<string, string> filter, int take, int skip);
        //Task<ApplicationUserDto> GetUser(string id);
        //Task<ApplicationUserDto> GetUserByName(string name);
        Task<ApplicationUserEntity> CreateUser(ApplicationUserDto dto, string password);
        Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto dto);
        //Task<ApplicationUserDto> GetUserProfile(string name);
        //Task<ApplicationUserDto> UpdateUserProfile(string userId, ApplicationUserProfileDto model);
        //Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword);
        Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe);
        Task SignInAsync(ApplicationUserEntity entity, bool isPersistent);
        Task SignOutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserEntity entity);
    }

    public class AccountBusinessService: IAccountBusinessService {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUserEntity> _signInManager;

        private readonly IUserProfileManager _userProfileManager;

        public AccountBusinessService(IMapper mapper,
            UserManager<ApplicationUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUserEntity> signInManager,
            IUserProfileManager userProfileManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userProfileManager = userProfileManager;
        }

        public async Task<ApplicationUserEntity> CreateUser(ApplicationUserDto dto, string password) {
            var user = new ApplicationUserEntity() {
                UserName = dto.UserName,
                NormalizedUserName = dto.NormalizedUserName,
                Email = dto.Email,
                EmailConfirmed = dto.EmailConfirmed,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberConfirmed = dto.PhoneNumberConfirmed
            };

            try {
                var result = await _userManager.CreateAsync(user, password);

                if(result.Succeeded) {
                    // Add user to new roles
                    // var roleNames = await _roleManager.Roles.Where(x => model.Roles.Contains(x.Id)).Select(x => x.Name).ToArrayAsync();
                    //var res2 = await _userManager.AddToRolesAsync(user.Id, roleNames);
                    return user;
                } else {
                    foreach(var error in result.Errors) {
                        //ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            } catch(Exception er) {
                System.Console.WriteLine(er.Message);
            }
            return null;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUserEntity entity) {
            return await _userManager.GenerateEmailConfirmationTokenAsync(entity);
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe) {
            return await _signInManager.PasswordSignInAsync(userName, password, rememberMe, false);
        }

        public async Task SignInAsync(ApplicationUserEntity entity, bool isPersistent) {
            await _signInManager.SignInAsync(entity, isPersistent);
        }

        public async Task SignOutAsync() {
            await _signInManager.SignOutAsync();
        }

        public async Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto dto) {
            try {
                var item1 = await _userProfileManager.Create(new UserProfileEntity() {
                    Name = "test3",
                    SurName = "test3",
                    MiddleName = "test3",
                    Uin = "44444"
                });
                return null;
            } catch(Exception e) {
                System.Console.WriteLine(e.Message);
            }
            return null;
            /*
            var item = await _userManager.FindByIdAsync(id);
            if(item == null) {
                return null;
            }

            var profile = await _userProfileManager.Find(item.Profile_Id);
            if(profile == null) {
                profile = new UserProfileEntity();
            }

            profile.Name = model.Name;
            profile.SurName = model.SurName;
            profile.MiddleName = model.MiddleName;
            profile.Uin = model.Uin;

            if(profile.Id == 0) {
                profile = await _userProfileManager.Create(profile);

                if(item != null) {
                    item.Profile_Id = profile.Id;
                    await _userManager.UpdateAsync(item);
                }
            } else {
                profile = await _userProfileManager.UpdateType(profile);
            }

            return _mapper.Map<ApplicationUserDto>(item);*/
        }
    }
}
