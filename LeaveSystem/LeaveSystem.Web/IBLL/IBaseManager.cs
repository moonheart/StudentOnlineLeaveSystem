using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IBLL
{
    public interface IBaseManager<TEntity> : IDisposable where TEntity : Entity<int>
    {
        Task<TEntity> AddEntityAsync(TEntity entity);
        TEntity AddEntity(TEntity entity);

        Task<IEnumerable<TEntity>> AddEntitiesAsync(ICollection<TEntity> entities);
        IEnumerable<TEntity> AddEntities(ICollection<TEntity> entities);

        Task UpdateEntityAsync(TEntity entity);
        void UpdateEntity(TEntity entity);

        Task UpdateEntitiesAsync(ICollection<TEntity> entities);
        void UpdateEntities(ICollection<TEntity> entities);

        Task DeleteEntityAsync(TEntity entity);
        void DeleteEntity(TEntity entity);

        Task DeleteEntitiesAsync(ICollection<TEntity> entities);
        void DeleteEntities(ICollection<TEntity> entities);

        Task<TEntity> FindEntityByIdAsync(int id);
        TEntity FindEntityById(int id);

        IQueryable<TEntity> GetAllEntities();

        IQueryable<TEntity> GetEntitiesByLamda(Expression<Func<TEntity, bool>> lamdaExpression);

        Task<bool> IsEntityExistByLamdaAsync(Expression<Func<TEntity, bool>> lamdaExpression);
        bool IsEntityExistByLamda(Expression<Func<TEntity, bool>> lamdaExpression);

    }
}
