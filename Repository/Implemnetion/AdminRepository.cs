using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class AdminRepository : BaseRepository<Users>, IAdminRepository
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
            var Data = await Find(x => x.UserEmail == userRequest.EmailOrName || x.UserName == userRequest.EmailOrName);


            if (Data == null)
                return null; 

            if (Data.Rule != "Admin")
                throw new ArgumentException("Access denied. Only admin users can log in.");

            var isPasswordValid = _AuthService.VerifyPassword(userRequest.Password,Data.PasswordHash,Data.PasswordSlat);

            if (!isPasswordValid)
                return null; 

            var token = _AuthService.GenerateUserToken(Data);
            return token;
        }

        public async Task<string> Register(UserRegisterRequest userRequest)
        {
            var existingUsername = await _context.users.AnyAsync(u => u.UserName == userRequest.UserName);
            if (existingUsername)
            {
                throw new Exception("Username is already taken.");
            }

            var existingEmail = await _context.users.AnyAsync(u => u.UserEmail == userRequest.UserEmail);
            if (existingEmail)
            {
                throw new Exception("Email is already taken.");
            }

            _AuthService.CreatePasswordHash(userRequest.Password, out string hash, out string salt);
            Users AdminData =
            new Users()
            {
                UserName = userRequest.UserName,
                UserEmail = userRequest.UserEmail,
                PasswordHash = hash,
                PasswordSlat = salt,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Rule = "Admin",

            };

            var data = await Add(AdminData);
            await _context.SaveChangesAsync();
            var Token = _AuthService.GenerateUserToken(AdminData);
            return Token;

        }
    }
}
