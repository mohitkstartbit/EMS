using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddData(T entity);
        Task<IEnumerable<T>> AddMultipleData(IEnumerable<T> entities);
        Task<T> DeleteData(int id);
        Task<IEnumerable<T>> DeleteMultipleData(IEnumerable<int> ids);
        Task<T> EditData(T value);
        Task<IEnumerable<T>> EditMultipleData(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllByExpression(Expression<Func<T, bool>> expression);
        Task<T> GetByExpression(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetData();
        Task<T> GetDataById(int id);
    }
}