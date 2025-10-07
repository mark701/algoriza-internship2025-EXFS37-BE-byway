using Domain.Enum;
using Domain.Models.DataBase.AdminPersona;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Extentions;
using Repository.Interface;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Algoriza_Project.Controllers.adminPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class instructorController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        public instructorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromForm] InstructorRequest instructor)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.instructors.AddInstructor(instructor);

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
        public async Task<IActionResult> Update([FromForm] InstructorRequest instructor)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.instructors.UpdateInstructor(instructor);

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
                var Data = await _unitOfWork.instructors.Find(x => x.instructorID == ID);
                var IsDeleted = await _unitOfWork.instructors.Delete(ID);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.instructors.DeleteImage(Data.instructorImagePath);
                return Ok(IsDeleted);
            }
            catch (SqlException ex)
            {

                return NotFound(ex.Message);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("DELETE") == true)
            {
                return Conflict("Cannot delete this instructor because it have 1 or more Course in The courses.");
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
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

                var Data = await _unitOfWork.instructors.Count(null);


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
        public async Task<IActionResult> GetCountCondation(string criteria = null)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                //JobTitleType? jobTitleEnum = Enum.TryParse<JobTitleType>(criteria, true, out var parsedEnum)? parsedEnum: (JobTitleType?)null;
                //JobTitleType? jobTitleEnum = CustomEnum<JobTitleType>.GetEnumValue(criteria);

                Expression<Func<instructors, bool>>? filter = null;

                if (!criteria.IsNullOrEmpty())
                {
                    filter = x => x.instructorName == criteria
                               || x.jobTitle.JobTilteName == criteria;
                }

                var count = await _unitOfWork.instructors.Count(filter);
                return Ok(count);

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

        [HttpGet("GetID_Name")]
        public async Task<IActionResult> GetID_NameInstructor()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {

                var Data = await _unitOfWork.instructors.GetSomeColumns(x => new instructors
                {
                    instructorID = x.instructorID,
                    instructorName = x.instructorName
                });

                var result = Data.Select(x => new { x.instructorID, x.instructorName }).ToList();

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


        [HttpGet("GetPages")]
        public async Task<IActionResult> GetPages(int pageNumber, int pageSize, string criteria = null)
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                var filters = new List<Expression<Func<instructors, bool>>>();


                if (!criteria.IsNullOrEmpty())
                {
                    filters.Add(x => x.instructorName == criteria || x.jobTitle.JobTilteName == criteria
                    || x.instructorName.Contains(criteria) || x.jobTitle.JobTilteName.Contains(criteria)


                    );
                }
                var includes = new List<Expression<Func<instructors, object>>>
                {
                    x => x.jobTitle
                };


                var (totalCount, data) = await _unitOfWork.instructors.GetPagesAsync(pageNumber, pageSize, filters, includes);


                return Ok(new
                {
                    TotalCount = totalCount,
                    Data = data.Select(x => new
                    {
                        x.instructorID,
                        x.instructorName,
                        x.courseRate,
                        x.instructorDescription,
                        x.instructorImagePath,
                        x.jobTitleID,
                        JobTitleName = x.jobTitle.JobTilteName
                    })
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
        [HttpGet("GetTopInstructors")]
        public async Task<IActionResult> GetTopInstructors()
        {
            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



            try
            {
                var Data = await _unitOfWork.instructors.GetInstructorStudentsCount();
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
