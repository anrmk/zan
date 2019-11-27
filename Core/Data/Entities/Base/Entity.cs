using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Base {
    /// <summary>
    /// Базовая сущность
    /// </summary>
    public abstract class BaseEntity {
        /// <summary>
        /// Код сущности
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Наименование сущности
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Сущность
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public abstract class Entity<T>: BaseEntity, IEntity<T> {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual T Id { get; set; }
    }
}
