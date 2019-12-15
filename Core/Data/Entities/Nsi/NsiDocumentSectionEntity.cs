using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Раздел законодательства
    /// </summary>
    [Table(name: "nsi.DocumentSections")]
    public class NsiDocumentSectionEntity: NsiEntity<Guid> {

    }
}
