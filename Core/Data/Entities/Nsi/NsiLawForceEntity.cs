using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Юридическая сила акта
    /// </summary>
    [Table(name: "nsi.LasForces")]
    public class NsiLawForceEntity: NsiEntity<Guid> {
    }
}
