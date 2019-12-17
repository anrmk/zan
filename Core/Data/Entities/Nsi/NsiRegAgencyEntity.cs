using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Орган госрегистрации
    /// </summary>
    [Table(name: "nsi.RegAgencies")]
    public class NsiRegAgencyEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
    }
}
