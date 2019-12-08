using System.ComponentModel.DataAnnotations.Schema;
using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Язык
    /// </summary>
    [Table(name: "nsi.Languages")]
    public class NsiLanguageEntity: NsiEntity<int> {
        public string CodeImport { get; set; }
    }
}
