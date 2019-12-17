using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Орган разработчик
    /// </summary>
    [Table(name: "nsi.DevAgencies")]
    public class NsiDevAgencyEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
    }
}
