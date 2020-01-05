using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Data.Entities.Base {
    /// <summary>
    /// Базовая сущность
    /// включает в себя дополнительные поля NAME и CODE 
    /// </summary>
    public abstract class BaseEntity<T>: IEntity<T> {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual T Id { get; set; }

        /// <summary>
        /// Код сущности
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Наименование сущности
        /// </summary>
        public string Name { get; set; }
    }
}
