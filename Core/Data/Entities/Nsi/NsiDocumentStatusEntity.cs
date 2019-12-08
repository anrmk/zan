using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Статус документа
    /// </summary>
    [Table(name: "nsi.DocumentStatuses")]
    public class NsiDocumentStatusEntity : NsiEntity<int> {
        public string CodeBd7 { get; set; }
    }
}
