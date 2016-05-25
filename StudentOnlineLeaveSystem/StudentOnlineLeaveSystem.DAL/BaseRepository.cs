using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using StudentOnlineLeaveSystem.IDAL;
using StudentOnlineLeaveSystem.Models;

namespace StudentOnlineLeaveSystem.DAL
{
    /// <summary>
    /// 仓库基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected StudentOnlineLeaveSystemDbContext SContext = ContextFactory.GetCurrentContext();
        public T Add(T entity)
        {
            SContext.Entry<T>(entity).State = EntityState.Added;
            SContext.SaveChanges();
            return entity;
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return SContext.Set<T>().Count(predicate);
        }

        public bool Update(T entity)
        {
            SContext.Set<T>().Attach(entity);
            SContext.Entry<T>(entity).State = EntityState.Modified;
            return SContext.SaveChanges() > 0;
        }

        public bool Delete(T entity)
        {
            SContext.Set<T>().Attach(entity);
            SContext.Entry<T>(entity).State = EntityState.Deleted;
            return SContext.SaveChanges() > 0;
        }

        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return SContext.Set<T>().Any(anyLambda);
        }

        public T Find(Expression<Func<T, bool>> whereLambda)
        {
            T entity = SContext.Set<T>().FirstOrDefault<T>(whereLambda);
            return entity;
        }

        public IQueryable<T> Entities
        {
            get { return SContext.Set<T>(); }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">原IQueryable</param>
        /// <param name="propertyName">排序属性名</param>
        /// <param name="isAsc">是否正序</param>
        /// <returns>排序后的IQueryable<T></returns>
        private IQueryable<T> OrderBy(IQueryable<T> source, string propertyName, bool isAsc)
        {
            if (source == null)
                throw new ArgumentNullException("source", "不能为空");
            if (string.IsNullOrEmpty(propertyName))
                return source;
            var parameter = Expression.Parameter(source.ElementType);
            var property = Expression.Property(parameter, propertyName);
            if (property == null)
                throw new ArgumentNullException("propertyName", "属性不存在");
            var lambda = Expression.Lambda(property, parameter);
            var methodName = isAsc ? "OrderBy" : "OrderByDescending";
            var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { source.ElementType, property.Type }, source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        public IQueryable<T> FindList<S>(Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc)
        {
            var list = SContext.Set<T>().Where<T>(whereLamdba);
            list = OrderBy(list, orderName, isAsc);
            return list;
        }

        public IQueryable<T> FindPageList<S>(int pageIndex, int pageSize, out int totalRecord, Expression<Func<T, bool>> whereLamdba, string orderName, bool isAsc)
        {
            var list = SContext.Set<T>().Where<T>(whereLamdba);
            totalRecord = list.Count();
            list = OrderBy(list, orderName, isAsc).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return list;
        }
    }
}
