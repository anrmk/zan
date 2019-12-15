using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Кроме актов
    /// </summary>
    [Table(name: "nsi.DocumentTitlePrefixes")]
    public class NsiDocumentTitlePrefixEntity: NsiEntity<Guid> {
        public string CodeRu { get; set; }
        public string CodeKk { get; set; }
        public string CodeEn { get; set; }
    }
}
