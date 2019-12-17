using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Форма акта
    /// </summary>
    [Table(name: "nsi.DocumentTypes")]
    public class NsiDocumentTypeEntity: NsiEntity<Guid> {
    }
}