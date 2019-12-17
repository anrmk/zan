using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Статус документа
    /// </summary>
    [Table(name: "nsi.DocumentStatuses")]
    public class NsiDocumentStatusEntity: NsiEntity<int> {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public string CodeBd7 { get; set; }
    }
}
