using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Data.Entities.Base {
    public abstract class NsiEntity<T>: Entity<T> {
        public string NameRu { get; set; }
        public string NameKk { get; set; }
        public string NameEn { get; set; }
        public int? Sort { get; set; }
    }
}
