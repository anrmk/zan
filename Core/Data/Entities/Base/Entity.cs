using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Data.Entities.Base {
    /// <summary>
    /// Сущность
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public interface IEntity<T> {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        T Id { get; set; }
    }

    /// <summary>
    /// Сущность
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public abstract class Entity<T>: IEntity<T> {
        /// <summary>
        /// Первичный ключ
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual T Id { get; set; }
    }
}
