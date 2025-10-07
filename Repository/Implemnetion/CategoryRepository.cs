using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class CategoryRepository : BaseRepository<categories>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<categories> SaveDataWithImage(CategoryRequest category)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            string path = "Assets/category";

            string imagePath = await SaveImageAsync(category.Image, allowedExtensions, path);

            categories categoryData = new categories
            {
                categoryDescription = category.categoryDescription,
                categoryName = category.categoryName,
                categoryImagePath = imagePath
            };

            var data = await Add(categoryData);
            await _context.SaveChangesAsync();
            return data;
        }
    }

}
