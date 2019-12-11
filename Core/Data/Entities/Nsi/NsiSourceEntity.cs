using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Источник публикации
    /// </summary>
    [Table(name: "nsi.Sources")]
    public class NsiSourceEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
    }
}
