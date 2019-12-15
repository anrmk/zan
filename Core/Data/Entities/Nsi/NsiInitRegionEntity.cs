using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Место принятия
    /// </summary>
    [Table(name: "nsi.InitRegions")]
    public class NsiInitRegionEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
        public int? OldId { get; set; }
    }
}
