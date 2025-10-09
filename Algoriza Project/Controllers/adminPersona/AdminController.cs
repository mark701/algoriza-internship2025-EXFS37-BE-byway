using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Implemnetion;
using Repository.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Algoriza_Project.Controllers.adminPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService authService;
        public AdminController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            this.authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest userLogin)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {


                var token = await _unitOfWork.Admins.Login(userLogin);


                if (token != null)
                {
                    return Ok(token);


                }
                return NotFound("Invalid email or password");
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterRequest adminRegister)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {



                //authService.CreatePasswordHash(adminRegister.Password, out string hash, out string salt);
                //Users AdminData =
                //new Users()
                //{
                //    UserName = adminRegister.UserName,
                //    UserEmail = adminRegister.UserEmail,
                //    PasswordHash = hash,
                //    PasswordSlat = salt,
                //    FirstName = adminRegister.FirstName,
                //    LastName = adminRegister.LastName,
                //    Rule= "Admin",

                //};

                //var data = await _unitOfWork.Admins.Add(AdminData);
                //await _unitOfWork.SaveChangesAsync();
                //var Token = authService.GenerateUserToken(AdminData);


                var Token = await _unitOfWork.Admins.Register(adminRegister);


                if (Token != null)
                {
                    return Ok(Token);


                }
                return NotFound();
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
