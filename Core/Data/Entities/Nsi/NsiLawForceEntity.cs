using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Юридическая сила акта
    /// </summary>
    [Table(name: "nsi.LasForces")]
    public class NsiLawForceEntity: NsiEntity<Guid> {
    }
}
