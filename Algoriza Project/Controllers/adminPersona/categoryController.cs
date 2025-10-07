using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Algoriza_Project.Controllers.adminPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class categoryController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public categoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromForm] CategoryRequest categoryRequest)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.categories.SaveDataWithImage(categoryRequest);
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

                var Data = await _unitOfWork.categories.GetAll();

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
        [HttpGet("GetCount")]
        public async Task<IActionResult> GetCount()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.categories.Count(null);


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

        [HttpGet("GetNameID")]
        public async Task<IActionResult> GetNameID()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.categories.GetSomeColumns(x => new categories
                {
                    categoryID = x.categoryID,
                    categoryName = x.categoryName
                });

                var result = Data.Select(x => new { x.categoryID, x.categoryName }).ToList();

                return Ok(result);


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


        [HttpGet("categoryCourseCounts")]
        public async Task<IActionResult> GetCategoryCourseCounts()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                var result = await _unitOfWork.categories.GetSomeColumns(selector: c => new
                {   c.categoryID,
                    c.categoryName,
                    c.categoryImagePath,
                    CourseCount = c.courses.Count()
                });
                return Ok(result.Take(10));
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
