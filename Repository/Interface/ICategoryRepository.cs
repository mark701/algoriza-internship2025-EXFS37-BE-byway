using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICategoryRepository : IBaseRepository<categories>
    {
        Task<categories> SaveDataWithImage(CategoryRequest category);
    } 
}
