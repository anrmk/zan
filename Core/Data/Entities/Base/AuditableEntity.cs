using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Base {
    /// <summary>
    /// Сушность с метками аудита
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public abstract class AuditableEntity<T>: Entity<T>, IAuditableEntity {
        /// <summary>
        /// Дата создания
        /// </summary>
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Пользователь-создатель
        /// </summary>
        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; } = "system";

        /// <summary>
        /// Дата обновления
        /// </summary>
        [ScaffoldColumn(false)]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Пользователь, обновивший запись
        /// </summary>
        [MaxLength(256)]
        [ScaffoldColumn(false)]
        public string UpdatedBy { get; set; } = "system";
    }
}
