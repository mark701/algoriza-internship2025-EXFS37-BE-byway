using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUserRepository : IBaseRepository<Users>
    {
        Task<string> Login(UserLoginRequest userRequest);


        //Task<(Users? UserData, string? token)>
        Task<string> Register(UserRegisterRequest userRequest);


        Task<string> SocialSign(SocialRequest googleRequest);


    }
}
