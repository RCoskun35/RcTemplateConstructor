using Application.Repositories;
using Domain.Entities.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public DbSet<T> Table => _context.Set<T>();
        private IDbContextTransaction _transaction;

        #region Read
        public async Task<IQueryable<T>> GetAllAsync(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return await Task.Run(() => query);
        }
        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public async Task<T> GetByIdAsync(int Id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();

            return await query.FirstOrDefaultAsync(data => data.Id == Id);
        }

        public async Task<IQueryable<T>> GetByIdListAsync(List<int> IdList, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return await Task.Run(() => query.Where(x => !x.IsDeleted && IdList.Contains(x.Id)));
        }

        public T GetById(int Id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();

            return query.FirstOrDefault(data => data.Id == Id);
        }
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = Table.AsNoTracking();

            return await query.SingleOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }

        public async Task<IQueryable<T>> GetAllProcAsync(string procedureName, List<SqlParameter> parameters, bool tracking = true)
        {
            var parameterString = string.Join(", ", parameters.Select(p => p.ParameterName));
            var command = $"EXEC {procedureName} {parameterString}";

            var query = Table
                .FromSqlRaw(command, parameters.ToArray());

            if (!tracking)
                query = query.AsNoTracking();

            return await Task.Run(() => query);
        }
        #endregion

        #region Write
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(entity);

            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> entities)
        {
            await Table.AddRangeAsync(entities);
            return true;
        }

        public bool Remove(T entity)
        {
            EntityEntry<T> entityEntry = Table.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            T entity = await Table.FirstOrDefaultAsync(data => data.Id == id);
            return Remove(entity);
        }

        public bool RemoveRange(List<T> entities)
        {
            Table.RemoveRange(entities);
            return true;
        }

        public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync();

        public bool Update(T entity)
        {
            EntityEntry<T> entityEntry = Table.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }

        public async Task<int> ExecProc(string procedureName, List<SqlParameter> parameters)
        {
            var parameterString = string.Join(", ", parameters.Select(p => p.ParameterName));
            var command = $"EXEC {procedureName} {parameterString}";
            var result = await _context.Database.ExecuteSqlRawAsync(command, parameters.ToArray());

            return result;
        }

        public async Task<List<TViewModel>> ExecProcResultJson<TViewModel>(string procedureName, List<SqlParameter> parameters)
        {
            var responseParameter = new SqlParameter("@response", SqlDbType.NVarChar, -1);
            responseParameter.Direction = ParameterDirection.Output;
            parameters.Insert(0, responseParameter);

            using (var sqlCommand = _context.Database.GetDbConnection().CreateCommand())
            {
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddRange(parameters.ToArray());

                if (sqlCommand.Connection.State != ConnectionState.Open)
                {
                    await sqlCommand.Connection.OpenAsync();
                }
                await sqlCommand.ExecuteNonQueryAsync();

                var responseValue = responseParameter.Value;
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TViewModel>>(responseValue.ToString());

                if (responseValue != null && responseValue != DBNull.Value)
                {
                    return result;
                }

                return new List<TViewModel>();
            }

        }
        public async Task<TViewModel> ExecProcResultJsonSingle<TViewModel>(string procedureName, List<SqlParameter> parameters)
        {
            var responseParameter = new SqlParameter("@response", SqlDbType.NVarChar, -1);
            responseParameter.Direction = ParameterDirection.Output;
            parameters.Insert(0, responseParameter);

            using (var sqlCommand = _context.Database.GetDbConnection().CreateCommand())
            {
                sqlCommand.CommandText = procedureName;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddRange(parameters.ToArray());

                await sqlCommand.Connection.OpenAsync();
                await sqlCommand.ExecuteNonQueryAsync();

                var responseValue = responseParameter.Value;
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TViewModel>(responseValue.ToString());


                return result;

            }
        }



        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }


        #endregion
    }
}
