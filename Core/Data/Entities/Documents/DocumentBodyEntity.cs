using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Documents {
    public class DocumentBodyEntity: BaseEntity<Guid> {
        [ForeignKey("Document")]
        [Column("DocumentEntity_Id")]
        public Guid? DocumentId { get; set; }
        public virtual DocumentEntity Document { get; set; }

        public string Body { get; set; }
    }
}
