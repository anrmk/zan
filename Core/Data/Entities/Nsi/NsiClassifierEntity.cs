using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Сфера правоотношений
    /// </summary>
    [Table(name: "nsi.Classifiers")]
    public class NsiClassifierEntity: NsiEntity<Guid> {
        public Guid? ParentId { get; set; }
    }
}
