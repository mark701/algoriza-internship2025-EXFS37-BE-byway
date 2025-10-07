using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Repository.Implemnetion
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {

        protected ApplicationDbContext  _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<T> Add(T entity)
        {
            await  _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRange(List<T> entity)
        {
            await _context.Set<T>().AddRangeAsync(entity);
            return entity;
        }

        public  T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return  entity;
        }



        public async Task<bool> Delete(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null) return false;
            _context.Set<T>().Remove(entity);
            return true;
        }

        public async Task<bool> Delete(Expression<Func<T, bool>> criteria)
        {
            var entity = await _context.Set<T>().FindAsync(criteria);

            if (entity == null) return false;
            _context.Set<T>().Remove(entity);
            return true;
        }
        public bool DeleteRange(List<T> entities)
        {
            if (entities == null || !entities.Any())
                throw new ArgumentException("No entities provided for deletion.");


            _context.Set<T>().RemoveRange(entities);
            return true;

        }


        public async Task<T> Find(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(criteria);

        }

        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.Where(criteria).ToListAsync();

        }

        public async Task<List<T>> GetAll()
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            return await query.ToListAsync<T>();
        }

        public async Task<List<TResult>> GetSomeColumns<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? criteria=null)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();

            if (criteria != null)
                query = query.Where(criteria); // apply filter only if it's not null

            return await query.Select(selector).ToListAsync();
        }


        public async Task<int> Count(Expression<Func<T, bool>>? criteria)
        {
            if (criteria== null)
                return await _context.Set<T>().CountAsync();

            return await _context.Set<T>().CountAsync(criteria);

        }

        public List<T> CompareDiffernce<TKey>(List<T> oldEntities, List<T> newEntities, Expression<Func<T, TKey>> propertySelector)
        {

                var oldPropertyValues = oldEntities.Select(propertySelector.Compile()).ToList();
                var newPropertyValues = newEntities.Select(propertySelector.Compile()).ToList();

                var deleteResultIDs = oldPropertyValues.Except(newPropertyValues).ToList();

                var entitiesToRemoveOrUpdate = oldEntities
                    .Where(entity => deleteResultIDs.Contains(propertySelector.Compile().Invoke(entity)))
                    .ToList();

                return entitiesToRemoveOrUpdate;
            
        }


        //Task<(int totalCount, List<T> data)>
        public async Task<(int totalCount, List<T> data)> GetPagesAsync(int pageNumber, int pageSize, List<Expression<Func<T, bool>>>? criterias = null, List<Expression<Func<T, object>>>? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            // Apply filters
            if (criterias != null)
            {
                foreach (var criteria in criterias)
                {
                    query = query.Where(criteria);
                }
            }

            // Apply includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            var totalCount = await query.CountAsync();
            // Apply pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //return  data;

            return (totalCount, data);
        }

        public async Task<decimal> GetMaxAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>>? criteria = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (criteria != null)
                query = query.Where(criteria);

            if (!query.Any())
                return 0; // or throw exception if you prefer

            return await query.MaxAsync(selector);
        }


        public async Task<string> SaveImageAsync(IFormFile imageFile,string[] AllowedExtensions, string subFolder)
        {
            
            string MainFolder = "wwwroot";

            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Invalid image file.");

            
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!AllowedExtensions.Contains(fileExtension))
                throw new ArgumentException($"Only {string.Join(", ", AllowedExtensions)} files are allowed.");


            
            var folderPath = Path.Combine(MainFolder, subFolder);
            Directory.CreateDirectory(folderPath);  

            
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            
            return Path.Combine(subFolder, uniqueFileName).Replace("\\", "/");
        }

        
        public void DeleteImage(string imagePath)
        {
            const string MainFolder = "wwwroot";

            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentException("Image path cannot be null or empty.");


            var fullPath = Path.Combine(MainFolder, imagePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

        }


    }
}
