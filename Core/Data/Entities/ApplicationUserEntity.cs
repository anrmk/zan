using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Core.Data.Entities {
    [Table(name: "ApplicationUsers")]
    public class ApplicationUserEntity: IdentityUser {
        [ForeignKey("Profile")]
        public long? Profile_Id { get; set; }
        public virtual UserProfileEntity Profile { get; set; }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUserEntity> manager) {
        //    return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //}
    }
}
