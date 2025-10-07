using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
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
    public class AdminRepository : BaseRepository<Admins>, IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuthService _AuthService;

        public AdminRepository(ApplicationDbContext context, IAuthService authService)
           : base(context)
        {
            _context = context;
            _AuthService = authService;

        }
        public async Task<string> Login(UserLoginRequest userRequest)
        {
            var Data = await Find(x => x.adminEmail == userRequest.EmailOrName || x.adminName == userRequest.EmailOrName);


            if (Data != null)
            {
                var IsVerifyPassword = _AuthService.VerifyPassword(userRequest.Password, Data.PasswordHash, Data.PasswordSlat);
                if (IsVerifyPassword)
                {
                    var token = _AuthService.GenerateAdminToken(Data);



                    return token;
                }
            }

            return null;
        }
    }
}
