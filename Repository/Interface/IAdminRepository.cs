using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IAdminRepository : IBaseRepository<Users>
    {
        Task<string> Login(UserLoginRequest userRequest);

        Task<string> Register(UserRegisterRequest userRequest);

    }
}
