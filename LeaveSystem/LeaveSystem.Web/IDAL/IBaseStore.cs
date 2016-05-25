using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Web.Models;

namespace LeaveSystem.Web.IDAL
{
    public interface IBaseStore<TEntity> : IDisposable where TEntity : Entity<int>
    {
        Task<TEntity> CreateEntityAsync(TEntity entity);

        TEntity CreateEntity(TEntity entity);

        Task<IEnumerable<TEntity>> CreateEntitiesAsync(ICollection<TEntity> entities);

        IEnumerable<TEntity> CreateEntities(ICollection<TEntity> entities);

        Task UpdateEntityAsync(TEntity entity);

        void UpdateEntity(TEntity entity);

        Task UpdateEntitiesAsync(ICollection<TEntity> entities);

        void UpdateEntities(ICollection<TEntity> entities);

        Task<TEntity> DeleteEntityAsync(TEntity entity);

        TEntity DeleteEntity(TEntity entity);

        Task<IEnumerable<TEntity>> DeleteEntitiesAsync(ICollection<TEntity> entities);

        IEnumerable<TEntity> DeleteEntities(ICollection<TEntity> entities);

        Task<TEntity> FindEntityByIdAsync(int id);

        TEntity FindEntityById(int id);

        IQueryable<TEntity> GetAllEntities();

        IQueryable<TEntity> GetEntitiesByLamda(Expression<Func<TEntity, bool>> lamdaExpression);

        Task<bool> IsEntityExistByLamdaAsync(Expression<Func<TEntity, bool>> lamdaExpression);

        bool IsEntityExistByLamda(Expression<Func<TEntity, bool>> lamdaExpression);

    }
}
