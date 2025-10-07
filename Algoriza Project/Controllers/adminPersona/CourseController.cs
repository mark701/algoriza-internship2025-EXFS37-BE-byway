using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Interface;
using System.Linq.Expressions;

namespace Algoriza_Project.Controllers.adminPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize]
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromForm] CoursesRequest coursesRequest)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.Courses.AddCourse(coursesRequest);

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
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] CoursesRequest coursesRequest)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.Courses.UpdateCourse(coursesRequest);

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


        [Authorize]
        [HttpDelete("Delete/{ID}")]
        public async Task<IActionResult> Delete(int ID)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                var Data = await _unitOfWork.Courses.Find(x => x.courseID == ID);
                _unitOfWork.Courses.DeleteImage(Data.CourseImagePath);
                var IsDeleted = await _unitOfWork.Courses.Delete(ID);
                await   _unitOfWork.SaveChangesAsync();

                return Ok(IsDeleted);
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("DELETE") == true)
            {
                return Conflict("Cannot delete this Course because it have 1 or more User Bought that Course.");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetInclude/{ID}")]
        public async Task<IActionResult> GetInclude(int ID)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                var Data = await _unitOfWork.Courses.GetInclude(ID);

                //var Data = await _unitOfWork.Courses.Find(x=>x.courseID==ID,x=>x.content);


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

                var Data = await _unitOfWork.Courses.Count(null);


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

        [HttpGet("GetCountCondation")]
        public async Task<IActionResult> GetCountCondation( string? criteria = null, string? category = null) 
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try

            {
                Expression<Func<Courses, bool>>? filter = null;
                if (!criteria.IsNullOrEmpty() && !category.IsNullOrEmpty())
                {
                    // Both criteria and category provided
                    filter = x => x.courseName == criteria && x.categories.categoryName == category;
                }
                else if (!criteria.IsNullOrEmpty())
                {
                    filter = x => x.courseName == criteria;
                }
                else if (!category.IsNullOrEmpty())
                {
                    filter = x => x.categories.categoryName == category;
                }

                var data = await _unitOfWork.Courses.Count(filter);

                return Ok(data);
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

        


        [HttpGet("GetPagesAdmin")]
        public async Task<IActionResult> GetPagesAdmin(int pageNumber, int pageSize, string? criteria,string? category)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                List<Expression<Func<Courses, bool>>> filters = new List<Expression<Func<Courses, bool>>>();

                if (!string.IsNullOrEmpty(criteria))
                {
                    filters.Add(x => x.courseName.Contains(criteria));
                }

                if (!string.IsNullOrEmpty(category))
                {
                    filters.Add(x => x.categories.categoryName == category);
                }



                var (totalCount, data) = await _unitOfWork.Courses.GetPagesAsync(pageNumber, pageSize, filters);


                return Ok(new
                {
                    TotalCount = totalCount,
                    Data = data
                }); ;
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



        [HttpPost("GetPagesUser")]
        public async Task<IActionResult> GetPagesUser(int pageNumber, int pageSize, [FromBody] FilterCoursesUser filter, string orderBy, bool ascending)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                Expression<Func<Courses, object>> orderExpression = orderBy switch
                {
                    "CoursePrice" => x => x.CoursePrice,
                    "CreateDateAndTime" => x => x.CreateDateAndTime,
                    _ => x => x.CreateDateAndTime 
                };



                var (totalCount, data) = await _unitOfWork.Courses.GetPagesFilterCourse(pageNumber, pageSize, filter, orderExpression, ascending);


                return Ok(new
                {
                    TotalCount = totalCount,
                    Data = data
                });
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


        [HttpGet("GetMaxPrice")]
        public async Task<IActionResult> GetMaxPrice()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {




                var Data = await _unitOfWork.Courses.GetMaxAsync(x => x.CoursePrice);


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

        [HttpGet("GetTopCourses")]
        public async Task<IActionResult> GetTopCourses()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                var Data = await _unitOfWork.Courses.GetCourseCount();
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

