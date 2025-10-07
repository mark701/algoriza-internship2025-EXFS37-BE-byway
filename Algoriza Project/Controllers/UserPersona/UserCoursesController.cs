using Domain.Models.DataBase.PaymentMethod;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Algoriza_Project.Controllers.UserPersona
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserCoursesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserCoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save(UserCourseRequest userCourse)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }


            try
            {

                var Data = await _unitOfWork.UserCoursesHeader.AddUserCourse(userCourse);

                return Ok(Data);
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

        [HttpGet("GetPricesWithDate")]
        public async Task<IActionResult> GetPricesWithDate(string TimeFilter)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }


            try
            {
                var now = DateTime.Now;


                if (TimeFilter.ToLower() == "month")
                {
                    var Data = await _unitOfWork.UserCoursesHeader.GetSomeColumns(x => new { x.Total, x.CreateDateAndTime },
                   x => x.CreateDateAndTime.Month == now.Month
                   && x.CreateDateAndTime.Year == now.Year);
                    return Ok(Data);
                }
                else if ((TimeFilter.ToLower() == "year"))
                {
                    var Data = await _unitOfWork.UserCoursesHeader.GetSomeColumns(x => new { x.Total, x.CreateDateAndTime }, x => x.CreateDateAndTime.Year == now.Year);
                    return Ok(Data);
                }



                return NotFound("No Data To send ");




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
