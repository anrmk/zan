using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Context {
    public interface IApplicationContext {
        Database ApplicationDatabase { get; }
        
        /// <summary>
        /// Получение набора данных
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <returns>Набор данных указанного типа</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Получение записи сущности
        /// </summary>
        /// <typeparam name="T">Тип возвращаемых данных</typeparam>
        /// <param name="entity">Сущность указанного типа</param>
        /// <returns>Запись сущности указанного типа</returns>
        EntityEntry<T> Entry<T>(T entity) where T : class;

        /// <summary>
        /// Асинхронное сохранение даных
        /// </summary>
        /// <returns>Результат исполнения</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Синхронное сохранение даных
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
