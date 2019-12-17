using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities {
    [Table(name: "UserProfiles")]
    public class UserProfileEntity: AuditableEntity<long> {
        /// <summary>
        /// ИИН
        /// </summary>
        [Required]
        [MaxLength(12)]
        public string Uin { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [MaxLength(64)]
        public string SurName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [MaxLength(64)]
        public string MiddleName { get; set; }
    }
}
