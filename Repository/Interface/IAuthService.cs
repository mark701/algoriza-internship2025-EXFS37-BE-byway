using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IAuthService
    {
        void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);
        bool VerifyPassword(string password, string storedHash, string storedSalt);

        //string GenerateAdminToken(Admins admins);
        string GenerateUserToken(Users users);

        string? GetClaim(string claimType);

    }
}
