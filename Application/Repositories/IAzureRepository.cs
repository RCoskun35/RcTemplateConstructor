using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IAzureRepository<TEntity>
    {
        Task CreateEntityAsync(TEntity entity);
        Task<TEntity> GetAsync(string rowKey);
        Task<List<TEntity>> GetAllAsync();
        Task UpdateEntityAsync(TEntity entity);
        Task DeleteEntityAsync(string rowKey);
    }
}
