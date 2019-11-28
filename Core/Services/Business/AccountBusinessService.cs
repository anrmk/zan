using System;
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
        Task<ApplicationUserEntity> CreateUser(ApplicationUserDto model);
        Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto model);
        //Task<ApplicationUserDto> GetUserProfile(string name);
        //Task<ApplicationUserDto> UpdateUserProfile(string userId, ApplicationUserProfileDto model);
        //Task<IdentityResult> UpdatePassword(string userId, string oldPassword, string newPassword);
    }

    public class AccountBusinessService: IAccountBusinessService {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserProfileManager _userProfileManager;

        public AccountBusinessService(IMapper mapper, 
            UserManager<ApplicationUserEntity> userManager, 
            RoleManager<IdentityRole> roleManager,
            IUserProfileManager userProfileManager) {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _userProfileManager = userProfileManager;
        }

        public async Task<ApplicationUserEntity> CreateUser(ApplicationUserDto model) {
            var user = new ApplicationUserEntity() {
                UserName = model.UserName,
                NormalizedUserName = model.NormalizedUserName,
                Email = model.Email,
                EmailConfirmed = model.EmailConfirmed,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = model.PhoneNumberConfirmed
            };
            try {
                var item = await _userManager.CreateAsync(user, "1234");

                if(item.Succeeded) {
                    // Add user to new roles
                    // var roleNames = await _roleManager.Roles.Where(x => model.Roles.Contains(x.Id)).Select(x => x.Name).ToArrayAsync();
                    //var res2 = await _userManager.AddToRolesAsync(user.Id, roleNames);
                }
                return item.Succeeded ? user : null;

            } catch (Exception er) {
                System.Console.WriteLine(er.Message);
            }
            return null;
        }

        public async Task<ApplicationUserDto> UpdateUserProfile(string id, UserProfileDto model) {
            try {
                var item1 = await _userProfileManager.Create(new UserProfileEntity() {
                    Name = "test",
                    SurName = "test",
                    MiddleName = "test",
                    Uin = "123123"
                });
                return null;
            }catch(Exception e) {
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
