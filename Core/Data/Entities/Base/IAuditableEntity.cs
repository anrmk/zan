using System;

namespace Core.Entities.Base {
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
}