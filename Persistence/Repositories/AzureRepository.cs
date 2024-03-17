using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;

namespace Persistence.Repositories
{
    public class AzureRepository<TEntity>:IAzureRepository<TEntity> where TEntity : class, ITableEntity, new()
    {
        private readonly TableServiceClient tableServiceClient;
        private readonly TableClient table;

        public AzureRepository()
        {
            this.tableServiceClient = new TableServiceClient(Configuration.AzureConnectionString);
            this.table = tableServiceClient.GetTableClient(typeof(TEntity).Name);
            this.table.CreateIfNotExists();
        }

        public async Task CreateEntityAsync(TEntity entity)
        {
            entity.RowKey = Guid.NewGuid().ToString();
            entity.PartitionKey=typeof(TEntity).Name;   
            await table.AddEntityAsync(entity);
        }

        public async Task<TEntity> GetAsync( string rowKey)
        {
            var response = await table.GetEntityAsync<TEntity>(typeof(TEntity).Name, rowKey);
            return response.Value;
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            List<TEntity> entities = new List<TEntity>();
            var result = table.QueryAsync<TEntity>(filter: "");
            await foreach (var item in result)
            {
                entities.Add(item);
            }
            return entities;
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            await table.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeleteEntityAsync( string rowKey)
        {

            await table.DeleteEntityAsync(typeof(TEntity).Name, rowKey);
        }
    }
}
