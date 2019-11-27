using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Context;
using Microsoft.EntityFrameworkCore;

namespace Core.Services.Base {
    /// <summary>
    /// Абстрактный сервис данных
    /// </summary>
    /// <typeparam name="T">Тип данных, с которым работает сервис</typeparam>
    public abstract class AsyncEntityService<T>: IEntityService<T> where T : class {
        protected IApplicationContext Context;

        public bool ShareContext { get; set; } = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст СУБД</param>
        protected AsyncEntityService(IApplicationContext context) {
            Context = context;
        }

        protected DbSet<T> DbSet => Context.Set<T>();

        /// <summary>
        /// Количество записей
        /// </summary>
        public virtual Task<int> Count => DbSet.CountAsync();

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <returns>Типизированный поставщик данных указанного типа</returns>
        public virtual async Task<IQueryable<T>> All() {
            try {
                var x = (await DbSet.ToListAsync()).AsQueryable();
                return x;
            } catch(Exception exception) {
                throw exception;
            }
        }
        /// <summary>
        /// Фильтровать записи по предикату
        /// </summary>
        /// <param name="predicate">Типизированный предикат</param>
        /// <returns>Типизированный поставщик данных указанного типа</returns>
        public virtual async Task<IQueryable<T>> Filter(Expression<Func<T, bool>> predicate) {
            return (await DbSet.Where(predicate).ToListAsync()).AsQueryable<T>();
        }

        /// <summary>
        /// Проверка наличия элемента в типизированном наборе данных
        /// </summary>
        /// <param name="predicate">Типизированный предикат</param>
        /// <returns>Типизированный поставщик данных указанного типа</returns>
        public async Task<bool> Contains(Expression<Func<T, bool>> predicate) {
            return await DbSet.CountAsync(predicate) > 0;
        }

        /// <summary>
        /// Получить запись по ключу/ключам
        /// </summary>
        /// <param name="keys">Ключ/ключи</param>
        /// <returns>Типизированный результат</returns>
        public virtual async Task<T> Find(params object[] keys) {
            return await DbSet.FindAsync(keys);
        }

        /// <summary>
        /// Получить запись по вычисляемому выражению
        /// </summary>
        /// <param name="predicate">Выражение предиката</param>
        /// <returns>Типизированный результат</returns>
        public virtual async Task<T> Find(Expression<Func<T, bool>> predicate) {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Сохранить типизированный параметр
        /// </summary>
        /// <param name="TObject">Типизированный параметр</param>
        /// <returns>Типизированный результат</returns>
        public virtual async Task<T> Create(T TObject) {
            try {
                var entry = DbSet.Add(TObject);
                if(!ShareContext)
                    await Context.SaveChangesAsync();

                return entry.Entity;
            } catch(Exception ex) {
                throw ex;
            }
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="t">Типизированный параметр</param>
        /// <returns>Целочисленный результат SaveChangesAsync</returns>
        public virtual async Task<int> Delete(T t) {
            DbSet.Remove(t);
            if(!ShareContext)
                return await Context.SaveChangesAsync();
            return 0;
        }

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="t">Типизированный параметр</param>
        /// <returns>Целочисленный результат SaveChangesAsync</returns>
        public virtual async Task<int> Update(T t) {
            var entry = Context.Entry(t);
            DbSet.Attach(t);
            entry.State = EntityState.Modified;
            if(!ShareContext)
                return await Context.SaveChangesAsync();
            return 0;
        }

        /// <summary>
        /// Обновить запись
        /// </summary>
        /// <param name="t">Типизированный параметр</param>
        /// <returns>Типизированный результат SaveChangesAsync</returns>
        public virtual async Task<T> UpdateType(T t) {
            var entry = Context.Entry(t);
            DbSet.Attach(t);
            entry.State = EntityState.Modified;
            if(!ShareContext)
                await Context.SaveChangesAsync();
            return t;
        }

        /// <summary>
        /// Обновить много записей
        /// </summary>
        /// <param name="t">Типизированный параметр</param>
        /// <returns>Типизированный результат SaveChangesAsync</returns>
        public virtual async Task<IEnumerable<T>> UpdateType(IEnumerable<T> l) {
            foreach(var t in l) {
                var entry = Context.Entry(t);
                DbSet.Attach(t);
                entry.State = EntityState.Modified;
                if(!ShareContext)
                    await Context.SaveChangesAsync();
            }
            return l;
        }

        /// <summary>
        /// Удалить запись
        /// </summary>
        /// <param name="predicate">Выражение предиката</param>
        /// <returns>Целочисленный результат SaveChangesAsync</returns>
        public virtual async Task<int> Delete(Expression<Func<T, bool>> predicate) {
            var objects = await Filter(predicate);
            foreach(var obj in objects)
                DbSet.Remove(obj);
            if(!ShareContext)
                return await Context.SaveChangesAsync();
            return 0;
        }

        /// <summary>
        /// Фильтрация с ограничением по количеству записей
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="where"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>Типизированный поставщик данных указанного типа</returns>
        public async Task<IQueryable<T>> Filter<Key>(Expression<Func<T, bool>> where, int offset, int limit) {
            int skipCount = offset * limit;
            var query = where is null ? DbSet.AsQueryable() : DbSet.Where(where).AsQueryable();
            query = skipCount == 0 ? query.Take(limit) : query.Skip(skipCount).Take(limit);

            return (await query.ToListAsync()).AsQueryable();
        }

        /// <summary>
        /// Пейджинг - постраничное получение списка
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="properties"></param>
        /// <returns>Типизированный поставщик данных указанного типа</returns>
        public async Task<Tuple<List<T>, int>> Pager<Key>(Expression<Func<T, bool>> where, Expression<Func<T, string>> order, bool descSort, int offset, int limit, params string[] properties) {
            var query = where is null ? DbSet.AsQueryable() : DbSet.Where(where).AsQueryable();
            int count = await query.CountAsync();

            if(order is null) {
                query = query.Skip(offset);
            } else {
                query = descSort ? query.OrderByDescending(order).Skip(offset) : query.OrderBy(order).Skip(offset);
            }

            foreach(var prop in properties)
                query = query.Include(prop);

            query = query.Take(limit);
            var result = await query.ToListAsync();
            return new Tuple<List<T>, int>(result, count);
        }

        public async Task<Tuple<List<T>, int>> Pager<Key>(Expression<Func<T, bool>> where, Expression<Func<T, string>> order, int offset, int limit, params string[] properties) {
            return await Pager<Key>(where, order, false, offset, limit, properties);
        }
    }
}
