using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Ведомства
    /// </summary>
    [Table(name: "nsi.Departments")]
    public class NsiDepartmentEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
    }
}
