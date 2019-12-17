using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Core.Data.Entities.Base;

namespace Core.Data.Entities.Nsi {
    /// <summary>
    /// Язык
    /// </summary>
    [Table(name: "nsi.Languages")]
    public class NsiLanguageEntity: NsiEntity<int> {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        public string CodeImport { get; set; }
    }
}
