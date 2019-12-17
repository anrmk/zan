using System;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// 
    /// </summary>
    [Table(name: "nsi.GrifTypes")]
    public class NsiGrifTypeEntity: NsiEntity<Guid> {
    }
}
