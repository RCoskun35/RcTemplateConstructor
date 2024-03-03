using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Table { get; }

        #region Read
        Task<IQueryable<T>> GetAllAsync(bool tracking = true);
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        Task<IQueryable<T>> GetByIdListAsync(List<int> IdList, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(int id, bool tracking = true);
        T GetById(int id, bool tracking = true);
        Task<IQueryable<T>> GetAllProcAsync(string procedureName, List<SqlParameter> parameters, bool tracking = true);

        #endregion

        #region Write
        Task<bool> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        bool Remove(T entity);
        bool RemoveRange(List<T> entities);
        Task<bool> RemoveAsync(int id);
        bool Update(T entity);
        Task<int> SaveAsync();
        Task<int> ExecProc(string procedureName, List<SqlParameter> parameters);
        Task<List<TViewModel>> ExecProcResultJson<TViewModel>(string procedureName, List<SqlParameter> parameters);
        Task<TViewModel> ExecProcResultJsonSingle<TViewModel>(string procedureName, List<SqlParameter> parameters);


        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        #endregion
    }
}
