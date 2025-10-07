using Domain.Models.DataBase.AdminPersona;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Repository.Interface;

namespace Algoriza_Project.Controllers.UserPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

                var  token = await _unitOfWork.Users.Login(userLogin);


                if (token != null)
                {
                    return Ok(token);
              

                }
                return Unauthorized("Invalid email or password.");
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
        public async Task<IActionResult> Register(UserRegisterRequest userRegister)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var  token  = await _unitOfWork.Users.Register(userRegister);


                if (token != null)
                {
                    return Ok(token);


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

        [HttpPost("SocialSign")]
        public async Task<IActionResult> GoogleSign(SocialRequest socialSignRequest)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var token = await _unitOfWork.Users.SocialSign(socialSignRequest);


                if (token != null)
                {
                    return Ok(token);


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
