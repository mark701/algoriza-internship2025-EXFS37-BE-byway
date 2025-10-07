using Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Algoriza_Project.Controllers.adminPersona
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        [HttpGet("courseLevels")]
        public IActionResult GetCourseLevels()
        {
            var values = Enum.GetValues(typeof(CourseLevelType)).Cast<CourseLevelType>().Select(e => new
            {
                id = (int)e,
                name = e.ToString()
            });

            return Ok(values);
        }
    }
}
