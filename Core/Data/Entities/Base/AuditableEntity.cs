using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Entities.Base {
    public interface IAuditableEntity {
        /// <summary>
        /// Дата создания
        /// </summary>
        DateTime CreatedDate { get; set; }
        /// <summary>
        /// Пользователь-создатель
        /// </summary>
        string CreatedBy { get; set; }
        /// <summary>
        /// Дата обновления
        /// </summary>
        DateTime UpdatedDate { get; set; }
        /// <summary>
        /// Пользователь, обновивший запись
        /// </summary>
        string UpdatedBy { get; set; }
    }

    /// <summary>
    /// Сушность с метками аудита
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public abstract class AuditableEntity<T>: BaseEntity<T>, IAuditableEntity {
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
