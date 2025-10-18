using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Repository.Extentions;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class UserRepository : BaseRepository<Users>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IAuthService _AuthService;
        private readonly IEmailService _emailService;
        public UserRepository(ApplicationDbContext context, IAuthService authService, IEmailService emailService)
            : base(context)
        {
            _context = context;
            _AuthService = authService;
            _emailService = emailService;
        }


        public async Task<string> Login(UserLoginRequest userRequest)
        {
            var Data = await Find(x => x.UserEmail == userRequest.EmailOrName || x.UserName == userRequest.EmailOrName);


            if (Data != null)
            {
                var IsVerifyPassword = _AuthService.VerifyPassword(userRequest.Password, Data.PasswordHash, Data.PasswordSlat);
                if (Data.Rule != "User")
                    throw new ArgumentException("Access denied. Only  users can log in.");

                if (IsVerifyPassword)
                {
                    var token = _AuthService.GenerateUserToken(Data);



                    return token;
                }
            }

            
            return null;
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

            Users UserData =
            new Users()
            {
                UserName = userRequest.UserName,
                UserEmail = userRequest.UserEmail,
                FirstName= userRequest.FirstName, 
                LastName= userRequest.LastName,
                PasswordHash = hash,
                PasswordSlat = salt,
                Rule= "User",

            };
            
            var Data = await Add(UserData);
            await _context.SaveChangesAsync();
            var Token = _AuthService.GenerateUserToken(Data);


            return Token;

        }

        public async Task<string> SocialSign(SocialRequest socialSignRequest)
        {
            Users UserData =
            new Users()
            {
                UserName = socialSignRequest.UserName,
                UserEmail = socialSignRequest.UserEmail,
                FirstName = socialSignRequest.FirstName,
                LastName = socialSignRequest.LastName,
                TypeSign = socialSignRequest.TypeSign,
                Rule= "User",

            };
            var Data = await Find(x => x.UserEmail == socialSignRequest.UserEmail);
            if (Data !=null)
            {
                UserData.UserID = Data.UserID;

            } else
            {

             
                var DataInsert = await Add(UserData);
             
                await SendWelcomeMsg(socialSignRequest.UserEmail, socialSignRequest.FirstName);
                await _context.SaveChangesAsync();
            }

            
            var Token = _AuthService.GenerateUserToken(UserData);


            return Token;

        }

        public async Task<List<Users>> UserSearch(string Search)
        {
            List<Expression<Func<Users, bool>>> filters = new List<Expression<Func<Users, bool>>>();

            // Always include the rule filter
            filters.Add(u => u.Rule == "User");

            if (!string.IsNullOrEmpty(Search))
            {
                // Search in FirstName, LastName, or UserEmail
                filters.Add(u =>
                    u.FirstName.Contains(Search) ||
                    u.LastName.Contains(Search) ||
                    u.UserEmail.Contains(Search)
                );
            }
            var (totalCount, data) = await GetPagesAsync(1, 8, filters);

            return data;


        }

        private async Task SendWelcomeMsg(string email, string name)
        {

            var SubjectMsg = $"✨ Welcome Aboard, {name}! Your Learning Journey Begins";

            var Message = $@"
                            <!DOCTYPE html>
                            <html>
                            <head>
                              <meta charset='UTF-8'>
                              <style>
                                body {{ font-family: Arial, sans-serif; color: #333; line-height: 1.6; }}
                                .container {{ max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 10px; background: #f9f9f9; }}
                                h1 {{ color: #4CAF50; }}
                                .TeamMsg {{ margin-top: 20px; font-size: 0.9em; color: #666; }}
                              </style>
                            </head>
                            <body>
                              <div class='container'>
                                <h1>Welcome aboard, {name}! 🎉</h1>
                                <p>Hi {name}, 👋</p>
                                <p>Your learning journey starts here — and we’re excited to grow your skills together. 🚀</p>
                                <p>Let’s take the first step towards new opportunities and success.</p>
                                <p><b>Keep learning, keep growing, and let’s make this an amazing experience! 💡</b></p>
                                <p class='TeamMsg'>Best regards,<br/>ByWay Team</p>
                              </div>
                            </body>
                            </html>";
         
               await _emailService.SendEmailAsync(email, SubjectMsg, Message);





        }
    }

}


