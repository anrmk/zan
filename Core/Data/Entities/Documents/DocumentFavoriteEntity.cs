using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Documents {
    public class DocumentFavoriteEntity: Entity<long> {
        [Column(name: "DocumentEntity_Id")]
        public Guid? DocumentId { get; set; }
        public virtual DocumentEntity Document { get; set; }

        [Column(name: "ApplicationUserEntity_Id")]
        public Guid? ApplicationUserId { get; set; }
        public virtual ApplicationUserEntity ApplicationUser { get; set; }
    }
}
