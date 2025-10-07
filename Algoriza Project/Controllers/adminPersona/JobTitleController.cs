using Domain.Models.DataBase.AdminPersona;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Repository.Interface;

namespace Algoriza_Project.Controllers.adminPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobTitleController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public JobTitleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost("Save")]
        public async Task<IActionResult> Save(JobTitle jobTitle)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.JobTitle.Add(jobTitle);
                await _unitOfWork.SaveChangesAsync();

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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.JobTitle.GetAll();

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


    }
}
