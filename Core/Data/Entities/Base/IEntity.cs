namespace Core.Entities.Base {
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
}
