using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Регион действий
    /// </summary>
    [Table(name: "nsi.Regions")]
    public class NsiRegionEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
        public string ComentRu { get; set; }
        public string ComentKk { get; set; }
        public string ComentEn { get; set; }
    }
}
