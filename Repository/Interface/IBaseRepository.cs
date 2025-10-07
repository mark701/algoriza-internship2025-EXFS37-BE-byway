using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IBaseRepository<T> where T : class
    
    {
        //Task<T> GetID(int id);

        Task<T> Add(T entity);

        Task<List<T>> AddRange(List<T> entity);
        Task<List<T>> GetAll();

        Task<T> Find(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes);

        Task<(int totalCount, List<T> data)> GetPagesAsync(int pageNumber, int pageSize, List<Expression<Func<T, bool>>>? criterias = null, List<Expression<Func<T, object>>>? includes = null);
        T Update(T entity);
        Task<bool> Delete(int id);

        Task<decimal> GetMaxAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? criteria = null);
        List<T> CompareDiffernce<TKey>(List<T> oldEntities, List<T> newEntities, Expression<Func<T, TKey>> propertySelector);
        Task<bool> Delete(Expression<Func<T, bool>> criteria);
        bool DeleteRange(List<T> entities);
        Task<int> Count(Expression<Func<T, bool>>? criteria);

        Task<List<TResult>> GetSomeColumns<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? criteria = null);

        Task<string> SaveImageAsync(IFormFile imageFile, string[] AllowedExtensions, string subFolder);

        void DeleteImage(string imagePath);




    }
}
